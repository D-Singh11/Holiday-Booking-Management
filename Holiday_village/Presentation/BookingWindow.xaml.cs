using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BusinessObjects;

namespace Presentation
{
    /// <summary>
   
    /// This class is opened from main window section but it handles all booking operations.
    /// It is used to add, update, delete and find bookingd.
    /// It is also responsible for adding, deleteing , updating and finding customers.
    /// It use s the Facade class to interact with the bussiness logic of this application
    /// </summary>
    
    public partial class BookingWindow : Window
    {
        // private instance of facade class
        private EntitiesOperationsFacade listOps = new EntitiesOperationsFacade();
        private List<Guest> thisBookinGuestList = new List<Guest>();
        Guest existingGuest = new Guest();
        Booking booking_result;
        static int autoBookingRef = 0;



        //************************* 
        //CONSTRUCTOR  of Bookingwindow class 
        //*************************//************************* 
		
        public BookingWindow()
        {
            InitializeComponent();
            cmbox_Customer.ItemsSource = listOps.pGetCustomersList;  // populate combo box

            // handles the visibility of controls and only display required controls
            // rest of the controlsbecome visible when buttons are clicked onthe form
            bt_save_changes.Visibility = Visibility.Hidden;
            bt_delete_guest.Visibility = Visibility.Hidden;
            bt_Update_Booking.Visibility = Visibility.Hidden;
            lb_customer_made_booking.Visibility = Visibility.Hidden;
            bt_Invoice.Visibility = Visibility.Hidden;

            hideCarHireControls();
        }




/////////////ADD NEW BOOKING ///////////

        // This event handler is used to create new booking
        // It takes all the values from the form and generate the booking if data is valid
        private void bt_book_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                    Booking newBooking = new Booking();
                    // assigns booking dates to varables
                    newBooking.pArrival_date = dp_arrival_date.SelectedDate.Value;
                    newBooking.pDeparture_date = dp_depart_date.SelectedDate.Value;
                    newBooking.pChalet_id = int.Parse(tb_chaletID.Text);  

                    if (listOps.pAllBooking_List == null)   // check if booking is avilable only if customer file is empty
                    {
                        MessageBox.Show("Chalet availabe");     // show error message
                    }
                    else
                    {
                        // this method checks if chalet is already booked or avalaible
                        
                        bool chaletAvailable = NewBookingChaletAvalable
                                                    ( dp_arrival_date.SelectedDate.Value.Date, 
                                                      dp_depart_date.SelectedDate.Value.Date, 
                                                      int.Parse(tb_chaletID.Text));

                        if (chaletAvailable == false)   // if not available then show error and through exception
                        {
                            MessageBox.Show("This chalet is already booked.\nPlease select different chalet.", "Chalet not available", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                            throw new Exception();
                        }
                    }
                    
                    
                    // check if extras are selected or not
                    if (chkbox_breakfast.IsChecked == true)
                    {
                        newBooking.pBreakfast = true;
                    }

                    if (chkbox_dinner.IsChecked == true)
                    {
                        newBooking.pDinner = true;
                    }

                    if (chkbox_CarHire.IsChecked == true)
                    {
                      
                        try
                        {
                           
                            
                            // checks if car hire dates are valid
                            if (dp_Car_start_date.SelectedDate.Value.Date < dp_arrival_date.SelectedDate.Value.Date || 
                                dp_Car_end_date.SelectedDate.Value.Date > dp_depart_date.SelectedDate.Value.Date)
                            {
                                
                               throw new Exception();

                            }

                            newBooking.pCarHire = true;
                            
                            // setting value to car hire attributes
                            newBooking.pHireStartDate = dp_Car_start_date.SelectedDate.Value;
                            newBooking.pHireEndDate = dp_Car_end_date.SelectedDate.Value;
                            newBooking.pDriver_FullName = tb_driver_name.Text;
                        }
                        catch (Exception)
                        {
                           // MessageBox.Show("Enter all required details for car hire.","Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            MessageBox.Show("Enter all required details for car hire.\n\nAlso check if your hiring dates are within booking dates range.", "Car hire details error", MessageBoxButton.OK, MessageBoxImage.Error);
                            throw;
                            
                        }
                       

                        
                    }
                    

                    // set customer value to customer variable inboking
                    newBooking.pCustomerInBooking = (Customer)cmbox_Customer.SelectedItem;
                   
                    newBooking.pGuest_ist = listOps.pGuest_List;   // assign guests to booking
                    listOps.guestCounInRange(newBooking.pGuest_ist);  // clear guest list for next booking


                    newBooking.pBooking_reference = ++autoBookingRef;   // only create booking ref if all above details are correct
                    listOps.add_Booking(newBooking);                    // add booking to booking list
                    UpdateChanges(listOps.pGetBookingList);            // update booking
                
                    lbox_Guests.Items.Clear();
                    
                   listOps.pGuest_List.Clear();  // this allow to add same guest with same passport number to different bookings, 
                                                //because a guest can be on more than one booking
                    ClearForm();
                    this.Close();
                    MessageBox.Show("New Booking with booking refrence " + newBooking.pBooking_reference 
                                        + " has been created succesfully.","Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    


            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Select a customer from existing list. OR \n\nCreate new customer through \"Customer Section\".", "No Customer selected", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
            catch (FormatException)
            {
                MessageBox.Show("Chalet id j cannot be blank/non-numeric value.\n\nIt must be between 1-10.", "Chalet Id error", MessageBoxButton.OK, MessageBoxImage.Error);
               
            }
           
            catch (ArgumentOutOfRangeException )
            {

                MessageBox.Show("Add minimum of 1 OR maximum 6 guests to booking.","Guests number error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException)
            {

                MessageBox.Show("Enter a valid chalet id.\n\nIt must be between 1-10.","Invalid Chalet Id",MessageBoxButton.OK, MessageBoxImage.Error);
            }
          
            catch (Exception )
            {
               // MessageBox.Show("Chalet id j cannot be blank/non-numeric value.");
                MessageBox.Show("Please enter correct arrival/departure dates","Booking dates error",MessageBoxButton.OK, MessageBoxImage.Error); 
            }
        }



////FIND EXISTING BOOKING AND DISPLAY DETAILS ON FORM

        private void bt_Find_Booking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int searchRef = int.Parse(tb_find_deleteBooking.Text);
                booking_result = listOps.findBooking(searchRef);

                if (booking_result == null)
                {
                    MessageBox.Show("No booking exist with provided booking reference.\nPlease enter a valid reference number.","Invalid search number", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    ClearForm();
                    cmbox_Customer.Visibility = Visibility.Hidden;

                    // assigns found details to booking form
                    lb_booking_refNo.Content = booking_result.pBooking_reference;
                    dp_arrival_date.SelectedDate = booking_result.pArrival_date.Date;
                    dp_depart_date.SelectedDate = booking_result.pDeparture_date.Date;
                    tb_chaletID.Text = booking_result.pChalet_id.ToString();
                    
                    lb_customer_made_booking.Visibility = Visibility.Visible;

                    // display customer details on form
                    lb_customer_made_booking.Content = "Customer Ref : " + booking_result.pCustomerInBooking.pcustomer_Number + "\nName : " 
                                                        + booking_result.pCustomerInBooking.pName;

                    listOps.pGuest_List = booking_result.pGuest_ist;    // assigns found guests details to list
                    
                    foreach (Guest rg in booking_result.pGuest_ist)
                    {
                        lbox_Guests.Items.Add(rg);          // add guest details to list box on form
                    }


                    // chevcks if extras were added or not at the time of booking and display them on form
                    if (booking_result.pBreakfast == true)
                    {
                        chkbox_breakfast.IsChecked = true;
                    }

                    if (booking_result.pDinner == true)
                    {
                        chkbox_dinner.IsChecked = true;
                    }


                    // assigns car hire details to controls if tey were selected at the time of booking
                    if (booking_result.pCarHire == true)
                    {
                        chkbox_CarHire.IsChecked = true;
                        showCarHireControls();
                        dp_Car_start_date.SelectedDate = booking_result.pHireStartDate.Date;
                        dp_Car_end_date.SelectedDate = booking_result.pHireEndDate.Date;
                        tb_driver_name.Text = booking_result.pDriver_FullName;
                    }

                    // handles controls visibility
                    bt_Update_Booking.Visibility = Visibility.Visible;
                    bt_book.Visibility = Visibility.Hidden;
                    bt_Clear_all.Visibility = Visibility.Hidden;
                    tb_chaletID.Focusable = false;
                    bt_Invoice.Visibility = Visibility.Visible;

                    MessageBox.Show("Booking found booking.\nDetails of booking are displayed on the form.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    

                }

            }
            catch (FormatException)
            {
                        // show error if invalid input was made in find/delete text box
                MessageBox.Show("Invalid input.\nPlease enter a valid number.", "Invalid search number", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                   //      shows error if file does  not exist
                MessageBox.Show("Customer data file doesnot exist.", "File not foud", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           

        }






//////////SAVE CHANGES TO EXISTING BOOKING
        private void bt_Update_Booking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               // saves  changes made to booking 
                booking_result.pArrival_date = dp_arrival_date.SelectedDate.Value;
                booking_result.pDeparture_date = dp_depart_date.SelectedDate.Value;
                booking_result.pChalet_id = int.Parse(tb_chaletID.Text);
              

                // save changes made to extras
                if (chkbox_breakfast.IsChecked == false)
                {
                    booking_result.pBreakfast = false;
                }
                else
                {
                    booking_result.pBreakfast = true;
                }

                if (chkbox_dinner.IsChecked == false)
                {
                    booking_result.pDinner = false;
                }
                else
                {
                    booking_result.pDinner = true;
                }

                // check car hire changes controls 
                if (chkbox_CarHire.IsChecked == false)
                {

                    booking_result.pCarHire = false;
                   

                }
                else
                {
                    try
                    {


                        // validate car hire details
                        if (dp_Car_start_date.SelectedDate.Value.Date < dp_arrival_date.SelectedDate.Value.Date || 
                            dp_Car_end_date.SelectedDate.Value.Date > dp_depart_date.SelectedDate.Value.Date)
                        {

                            throw new Exception();

                        }

                        // save changes accordingly made to extras
                        booking_result.pCarHire = true;
                        booking_result.pHireStartDate = dp_Car_start_date.SelectedDate.Value;
                        booking_result.pHireEndDate = dp_Car_end_date.SelectedDate.Value;
                        booking_result.pDriver_FullName = tb_driver_name.Text;
                    }
                    catch (Exception)
                    {
                        // MessageBox.Show("Enter all required details for car hire.","Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        MessageBox.Show("Enter all required/correct details for car hire.\n\nAlso check if your hiring dates are within booking dates range.",
                                            "Car hire details error", MessageBoxButton.OK, MessageBoxImage.Error);
                        throw;

                    }
                    
                }

               
                listOps.guestCounInRange(booking_result.pGuest_ist);  // check if guest count is within range
                UpdateChanges(listOps.pGetBookingList);        // update changes made to booking list
                
                ClearForm();

                // handles controls visibility
                bt_Update_Booking.Visibility = Visibility.Hidden;   
                bt_Invoice.Visibility = Visibility.Hidden;
                tb_chaletID.Focusable = true;
                bt_book.Visibility = Visibility.Visible;
                bt_Clear_all.Visibility = Visibility.Visible;
                

                MessageBox.Show("Booking changes has been updated.","Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Select a customer from existing list. OR \n\nCreate new customer through \"Customer Section\".", 
                                    "No Customer selected", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            catch (FormatException)
            {
                MessageBox.Show("Chalet id j cannot be blank/non-numeric value.\n\nIt must be between 1-10.", "Chalet Id error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);

            }

            catch (ArgumentOutOfRangeException)
            {

                MessageBox.Show("Add minimum of 1 OR maximum 6 guests to booking.", "Guests number error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentException)
            {

                MessageBox.Show("Enter a valid chalet id.\n\nIt must be between 1-10.", "Invalid Chalet Id", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            catch (Exception)
            {
                // MessageBox.Show("Chalet id j cannot be blank/non-numeric value.");
                MessageBox.Show("Please enter correct arrival/departure dates", "Booking dates error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }


/////////Event handler used to Delele existing Bookig

        private void bt_Delete_Booking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int deleteRef = int.Parse(tb_find_deleteBooking.Text);

                // ask for confirmation to delete booking
                MessageBoxResult confirDelete = MessageBox.Show("Are you sure you want to delete booking with booking reference " + 
                                                    deleteRef + " ? ", " Warning", MessageBoxButton.YesNo);

                if (confirDelete == MessageBoxResult.Yes)        // delete booking if exists
                {
                    Boolean deleted = listOps.delete_Booking(deleteRef);
                    if (deleted == true)
                    {
                        MessageBox.Show("Booking deleted.\n\nRemaining number of bookings : " + listOps.pAllBooking_List.Count, "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                        ClearForm();

                    }
                    else
                    {
                        MessageBox.Show("No booking found with this number", "Invalid search number", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
            catch (Exception)
            {

                MessageBox.Show("Enter a vlid bookin refernce number", "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);   // handles excption if it is not handled in above code and display error message
            }


        }



/////////// Method used to update any changes made to the booking  
        // return null but take booking list as parameter

        public void UpdateChanges(List<Booking> bookinglist)
        {
            listOps.Save_Bookings_State();
        }


       


/////////GUESTS CODE ///////////////////////////////////////////////////////////////////////


        ////////////// ADD new Guest /////////
        // ths event handler adds new guest to the guest list 
        private void bt_add_new_guest_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Guest newGuest = new Guest();
                newGuest.pName = tb_Guest_name.Text;
                newGuest.pAge = int.Parse(tb_Guest_age.Text);

                // checks if passport number already exist
                foreach (Guest existingG in listOps.pGuest_List)
                {
                    if (existingG.pPassport_number == tb_Guest_passportNo.Text.ToUpper().Trim())
                    {                
                        throw new Exception();  // throw exception if two guests o same booking have same passport number
                    }

                }
                // trim all the white space and convert it to upper case
                newGuest.pPassport_number = tb_Guest_passportNo.Text.ToUpper().Trim();
                
                lbox_Guests.Items.Add(newGuest);    // add guest to listbox
                listOps.pGuest_List.Add(newGuest);          // add guest to list


                // display confirmation method once new customer ois added
                MessageBox.Show("Guest with following details added." +
                            "\nName: " + newGuest.pName +
                            "\nAge:  " + newGuest.pAge +
                            "\nPassport No. : " + newGuest.pPassport_number, "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);

            }
           
            catch (ArgumentNullException)   // handles all exception which may occur in its try block
            {

                MessageBox.Show("Enter all required details for guest.","Guest Details Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FormatException)
            {

                MessageBox.Show("Guest age j cannot be blank or non-numeric.\n\nPlease enter a valid age between 1-101.", "Guest Age Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Please enter valid age.\n\nAge must be between 1-101", "Guest Age Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
          
            catch (Exception)
            {
                MessageBox.Show("Two guests cannot have same passport number.\n\nPlease enter correct passport number.", "Invalid Passport Number", MessageBoxButton.OK, MessageBoxImage.Error);
               
            }
        }






    //// This event handler Selects guest from list box and display details on text boxes above list box to make changes
        // to guest details if required
        private void lbox_Guests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // cleras controls
            tb_Guest_name.Clear();
            tb_Guest_age.Clear();
            tb_Guest_passportNo.Clear();
            existingGuest = new Guest();
            existingGuest = (Guest)lbox_Guests.SelectedItem;
            if (existingGuest != null)
            {
                // show selected details of guest on its controls
                tb_Guest_name.Text = existingGuest.pName;
                tb_Guest_age.Text = existingGuest.pAge.ToString();
                tb_Guest_passportNo.Text = existingGuest.pPassport_number;
                bt_save_changes.Visibility = Visibility.Visible;
                bt_delete_guest.Visibility = Visibility.Visible;
               // bt_add_new_guest.Visibility = Visibility.Hidden;

                
                
            }
            
            
        }

     




        //// this event handler is used to EDIT Existing Guest from list box guests and update listbox

        private void bt_save_changes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // save changes made to the existing guest detalils
                existingGuest.pName = tb_Guest_name.Text;
                existingGuest.pAge = int.Parse(tb_Guest_age.Text);
                existingGuest.pPassport_number = tb_Guest_passportNo.Text;
                bt_save_changes.Visibility = Visibility.Hidden;
                bt_add_new_guest.Visibility = Visibility.Visible;
                lbox_Guests.Items.Refresh();        // refresh the listbox after changes made
                MessageBox.Show("Guest deatls has been updated.","Guest Updated",MessageBoxButton.OK, MessageBoxImage.Information);

            }

            catch (ArgumentNullException)
            {

                MessageBox.Show("Enter all required details for guest.", "Guest Details Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FormatException)
            {

                MessageBox.Show("Guest age j cannot be blank or non-numeric.\n\nPlease enter a valid age between 1-101.", "Guest Age Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Please enter valid age.\n\nAge must be between 1-101", "Guest Age Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }

            catch (Exception)
            {
                MessageBox.Show("Two guests cannot have same passport number.\n\nPlease enter correct passport number.", "Invalid Passport Number", MessageBoxButton.OK, MessageBoxImage.Error);

            }
          

        }



        // this event handler for delete button deletes the selected guest from list and database
        private void bt_delete_guest_Click(object sender, RoutedEventArgs e)
        {
            string delete_PasspNo = tb_Guest_passportNo.Text.ToUpper().Trim();
            bool deleted = listOps.delete_Guest(delete_PasspNo);   // calls delete_Guest method of facade class and return true or false

          


            if (deleted == true)  // if true means deleted then it shows confirmation and update lists
            {
                lbox_Guests.Items.Remove(existingGuest);  // remove deleted guest from lisbox assell
                lbox_Guests.Items.Refresh();
                MessageBox.Show("Guest has been removed from booking.\n\n Remaining number of guests :  " + 
                    listOps.pGuest_List.Count,"Guest Removed",MessageBoxButton.OK, MessageBoxImage.Information);

                if (lbox_Guests.Items.Count == 0)  // if all guests are deleted then below  lines make add guest button visible
                {
                    bt_add_new_guest.Visibility = Visibility.Visible;
                    bt_save_changes.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                MessageBox.Show("Please select a guest to delete.\n\nOR Booking must have minimum 1 guest.\n\n Remaining number of guests :  " 
                                + listOps.pGuest_List.Count, "guest delete error", MessageBoxButton.OK, MessageBoxImage.Question);
            }
       
        }


        // method to clear guest controls
        public void ClearGuest()
        {
            tb_Guest_name.Clear();
            tb_Guest_age.Clear();
            tb_Guest_passportNo.Clear();
        }



//////// CLEAR BOOKING FORM
       // event handler to clear booking form
        private void bt_Clear_all_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }


        ////// Clear Booking Form Method////////////
        // this method is used to clear the controls on booking form and also handles visiblity of controls
        public void ClearForm()
        {
            dp_arrival_date.SelectedDate = null;
            dp_depart_date.SelectedDate = null;
            lb_booking_refNo.Content = "Auto generated";
            tb_chaletID.Clear();
           // clear controls to its defaul form 
            chkbox_breakfast.IsChecked = false;     //ubcheck breakfast checkbox
            chkbox_dinner.IsChecked = false;        // uncheck dinner checkbox
            chkbox_CarHire.IsChecked = false;       // uncheck carhire check box
            dp_Car_start_date.SelectedDate = null;
            dp_Car_end_date.SelectedDate = null;
            tb_driver_name.Clear();
            tb_Guest_name.Clear();
            tb_Guest_age.Clear();
            tb_Guest_passportNo.Clear();
            lbox_Guests.Items.Clear();

            tb_find_deleteBooking.Clear();
            bt_book.Visibility = Visibility.Visible;
            bt_Update_Booking.Visibility = Visibility.Hidden;
            lb_customer_made_booking.Visibility = Visibility.Hidden;
            cmbox_Customer.Visibility = Visibility.Visible;

            // car hire controls hiding 
            hideCarHireControls();

            
        }

        // event handler called when car hire check box is checked
        private void chkbox_CarHire_Checked(object sender, RoutedEventArgs e)
        {
            showCarHireControls();  // show car hire's other controls

        }


        // event handler for uncheck car hire
        private void chkbox_CarHire_Unchecked(object sender, RoutedEventArgs e)
        {
            hideCarHireControls();    // hde car hire's other control
        }


        // Method used to hide car hire control
        public void hideCarHireControls()
        {
            lb_start_date.Visibility = Visibility.Hidden;
            dp_Car_start_date.Visibility = Visibility.Hidden;

            lb_end_date.Visibility = Visibility.Hidden;
            dp_Car_end_date.Visibility = Visibility.Hidden;

            lb_driver_name.Visibility = Visibility.Hidden;
            tb_driver_name.Visibility = Visibility.Hidden;
        }


        // Method used to show car hire constrols
        public void showCarHireControls()
        {
            lb_start_date.Visibility = Visibility.Visible;
            dp_Car_start_date.Visibility = Visibility.Visible;

            lb_end_date.Visibility = Visibility.Visible;
            dp_Car_end_date.Visibility = Visibility.Visible;

            lb_driver_name.Visibility = Visibility.Visible;
            tb_driver_name.Visibility = Visibility.Visible;

        }


       

        // this event handler for invoice generater button.
        private void bt_Invoice_Click(object sender, RoutedEventArgs e)
        {
            // price list for booking componenets
            double chaletUnitPrice = 60;
            double guestUnitPrice = 25;
            double breakfastUnitPrice = 5;
            double dinnerUnitPrice = 10;
            double carHireUnitPrice = 50;

            // intialise intial prices for breakfast and dinner

            int breakfastCount = 0;
            int dinnerCount = 0;

            // count guest number
            int guestCount = booking_result.pGuest_ist.Count;
           
            // count days
            int daysCount = (booking_result.pDeparture_date - booking_result.pArrival_date).Days;
          
            // count total numeber of car hire days
            int carDayCount = (booking_result.pHireEndDate - booking_result.pHireStartDate).Days;
            
            // calculate total costs for componrnets of booking
            double TotalChaletCost = (60 + (guestCount * 25) * daysCount);
            double TotalBreakfastCost = 0;
            double TotalDinnerCost = 0;
            double TotalCarCost = 0;
            
            // checks if any extras were added to booking and calculate their total cost for each componenet of extras
            if (booking_result.pBreakfast == true)
            {
                TotalBreakfastCost = breakfastUnitPrice * (guestCount * daysCount);
                breakfastCount = guestCount;
            }

            if (booking_result.pDinner == true)
            {
                TotalDinnerCost = dinnerUnitPrice * (guestCount * daysCount);
                dinnerCount = guestCount;
            }

            if (booking_result.pCarHire == true)
            {
                TotalCarCost = carHireUnitPrice * carDayCount;
            }

            double bookingCostTotal = TotalChaletCost + TotalBreakfastCost + TotalDinnerCost + TotalCarCost;
            BookingInvoice openInvoice = new BookingInvoice();
            openInvoice.Owner = this;


            // populate labels for customer details section on the  booking  invoice window
            openInvoice.lb_Booking_Ref_No.Content = booking_result.pBooking_reference;
            openInvoice.lb_Customer_Ref.Content = booking_result.pCustomerInBooking.pcustomer_Number;
            openInvoice.lb_Customer_Name.Content = booking_result.pCustomerInBooking.pName;
            openInvoice.lb_Address.Content = booking_result.pCustomerInBooking.pAddress;

            // set lables on booking invoice window for Perice Per Person (PPN) for different componenets of booking
            // it also display "£" and "x" symbols as well
            openInvoice.lb_chalet_PPN.Content = "£" + chaletUnitPrice + " + " + guestUnitPrice + " x " + guestCount;
            openInvoice.lb_breakfast_PPN.Content = "£" + breakfastUnitPrice + " x " + breakfastCount;
            openInvoice.lb_dinner_PPN.Content = "£" + dinnerUnitPrice+ " x " + dinnerCount;
            openInvoice.lb_carHire_PPN.Content = "£"+ carHireUnitPrice + " x " + carDayCount;


            // sets lables for Numbers Of Nights(NON) section of the Cost Breakdown section on booking
            // invoice window
            openInvoice.lb_chalet_NON.Content = "   " + daysCount;
            openInvoice.lb_breakfast_NON.Content = "   " + daysCount;
            openInvoice.lb_dinner_NON.Content = "   " + daysCount;
            openInvoice.lb_carHire_NON.Content = "   " + carDayCount;


            // set labels for total amount/cost per component of booking on the form
            openInvoice.lb_chalet_Amount.Content = "   " + TotalChaletCost;
            openInvoice.lb_breakfast_Amount.Content = "   " + TotalBreakfastCost;
            openInvoice.lb_dinner_Amount.Content = "   " + TotalDinnerCost;
            openInvoice.lb_carHire_Amount.Content = "   " + TotalCarCost;

            // Display the over all total obtained by ading the cost of all components of booking
            openInvoice.lb_Total_BookingCost.Content = "£" + bookingCostTotal;
            openInvoice.ShowDialog();   // opens the new child invoice window and shows booking details of current selected(by finding through serach box)
            
           
        }


        // This method is used to check if chalet is avalibale on selected dates
        private bool NewBookingChaletAvalable(DateTime arrivalDate, DateTime departureDate, int chaledID)
        {
            foreach (Booking existingBooking in listOps.pAllBooking_List)  // iterate through all existing bookings
            {
                // if statement check if new booking dates and existing booking are overlaping or not forg selected chaletId number
                if (
                        (arrivalDate >= existingBooking.pArrival_date && departureDate <= existingBooking.pDeparture_date) && ((chaledID) == existingBooking.pChalet_id) ||
                        (arrivalDate <= existingBooking.pArrival_date && departureDate >= existingBooking.pArrival_date) && ((chaledID) == existingBooking.pChalet_id) ||
                        (arrivalDate <= existingBooking.pDeparture_date && departureDate >= existingBooking.pDeparture_date) && ((chaledID) == existingBooking.pChalet_id)
                    )
                        
                {
                    return false;
                }
            }
            return true;
        }


        // this event handler is used to clear the guest details from texts boxes
        private void bt_ClearGuestForm_Click(object sender, RoutedEventArgs e)
        {
            ClearGuest(); //clears the Guest details from form
        }

       

      
    }
}
