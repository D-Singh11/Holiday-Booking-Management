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
using DataLayer;
using BusinessObjects;


namespace Presentation
{
    /// <summary>
    /// Purpose: This class is used by the Customer window to create, delete, update and read customers using the private instance
	/// of EntitiesOperationsFacade class.
    /// </summary>
	
	
    public partial class CustomerWindow : Window
    {
       
       private EntitiesOperationsFacade listOps = new EntitiesOperationsFacade();
       Customer result;     // used to store find results so that they can be used for update button
      
        static int autoCustomerRef = 0;

        
        //************************* 
		//CONSTRUCTOR  of Customer window class 
		//*************************
        public CustomerWindow()
        {
            InitializeComponent();						// intialize wcustomer window settings
            bt_SaveChanges.Visibility = Visibility.Hidden;		// hides the Save Changes button used to save updated customer details
            bt_EditCustomer.Visibility = Visibility.Hidden;		// hides the Edit Customer button used to edit customer
        }

		
		
		//************************* 
		// EVENT HANDLERS  
		//*************************
		
		// Event handler for Cancel button used to close Customer window
        private void bt_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Event handler for Add Customer button used to create new customer 
        private void bt_Add_Customer_Click(object sender, RoutedEventArgs e)
        {
            try								// try block used to handle exceptions which may occur due to invalid data input
            {
                
                Customer c = new Customer();			// create new instance of customer
                c.pName = tb_CustomerName.Text;			// assign user input from text box to name property os customer class
                c.pAddress = tb_CustomerAddress.Text;	// assign user input from text box to Address property os customer class
              
                c.pcustomer_Number = ++autoCustomerRef;		// auto increment and assign crefernce to customer number property
                   
                listOps.add_customer(c);					// add new customer to list
                UpdateChanges(listOps.pGetCustomersList);				// update customer in file
                MessageBox.Show("Customer with following details added."+				// show Message box to confirm 
                                "\nCustomer number: " + c.pcustomer_Number + 
                                "\nName : " + c.pName +
                                "\nAddress:  "+c.pAddress,"Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
               

			   ClearForm(); 		// clear form
               

                
            }
            catch (ArgumentNullException)
            {
                // show error messge if all the customer details are not provided.
                MessageBox.Show("Please enter all details for customer", "Customer details error", MessageBoxButton.OK, MessageBoxImage.Error);  

            }
            catch (Exception ex)   // this catch block catches any unhandled exception not caught by above catch blocks
            {

                MessageBox.Show(ex.ToString(),"Customer error", MessageBoxButton.OK, MessageBoxImage.Error);  // display eror message for exception
            }
           

        }

     


// This event handler is used to find the existing customer using customer ref number in the search box
// it displays the search results on the same form but make non necessary controls non focusable
        private void bt_FindCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int searchRef = int.Parse(tb_find_deleteNo.Text);   // search number to find customer in customer list
                 result = listOps.findCustomer(searchRef);       // store and find customer from customer list
                if (result == null)
                {
                    MessageBox.Show("Customer with enetered number does not exist.\n\nPlease enter a valid nymber",
                                    "Invalid customerr", MessageBoxButton.OK, MessageBoxImage.Error);		// show error if customer not found
                }
                else
                {
                    ClearForm();						// else clear form and 
                   
				   
				   // show confirmation that customer is found from list
                    MessageBox.Show("Customer is found.\n\nDetails are displayed on form.\n" +
                                "\nCustomer number: " + result.pcustomer_Number +
                                "\nName : " + result.pName +
                                "\nAddress:  " + result.pAddress, "Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    lb_customerRef.Content = result.pcustomer_Number.ToString();   // dispay found customer number on form
                    
					
					// display other customer details on form 
                    tb_CustomerName.Text = result.pName;
                    tb_CustomerName.Focusable = false;
                    tb_CustomerAddress.Text = result.pAddress;
                    tb_CustomerAddress.Focusable = false;
                    bt_Add_Customer.Visibility = Visibility.Hidden;
                    bt_EditCustomer.Visibility = Visibility.Visible;
                    
                }


            }
            catch (FormatException)
            {
                // show error if customer search number is invalid 
                MessageBox.Show("You must enter a valid number.","Invalid search input", MessageBoxButton.OK, MessageBoxImage.Error);  
            }
            catch(Exception)
            {   // shows error if customer file does  not exist or customer is null
                MessageBox.Show("No customer with that number exist/ no file exist", "Customer error", MessageBoxButton.OK, MessageBoxImage.Error);       
            }
        }



        // this method is used to clear the form and and also manages the visibility of some controls
        private void ClearForm()
        {

            // clear/reset form
            lb_customerRef.Content="Auto Generated";
            tb_CustomerName.Clear();
            tb_CustomerAddress.Clear();
            tb_find_deleteNo.Clear();
            bt_SaveChanges.Visibility = Visibility.Hidden;
            bt_EditCustomer.Visibility = Visibility.Hidden;
            bt_Add_Customer.Visibility = Visibility.Visible;
            
        }

		

// This event handler is for Clearform button n to clear form
        private void bt_ClearForm_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();        // acll to clearForm method to reset form
            tb_CustomerName.Focusable = true;    // make customer name text box focusable again
            tb_CustomerAddress.Focusable = true; // make customer address text box focusable again
           
            
           
        }



// This is event handler is for DeleteCustomer vuton used to delete an existing customer 
// from the lists

        private void bt_DeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                int searchRef = int.Parse(tb_find_deleteNo.Text);

                if (listOps.findCustomer(searchRef) == null)  // check if customer with provided reference is null or not
                {
                    throw new ArgumentNullException();		// throw exception if null  or error occurs

                }
                else		// ask for confirmation to delete customer 
                {
                    MessageBoxResult confirmDelete = MessageBox.Show("Are you sure you want to delete customer with " 
																		+ searchRef 
																		+ " ref number from list ?", "Warning", MessageBoxButton.YesNo);

                    if (confirmDelete == MessageBoxResult.Yes)  // if user select yes then
                    {
                        Boolean deleted = listOps.delete_Customer(searchRef);  // apply deletion operation and store returned value
                        if (deleted == true)
                        {
                            ClearForm();
                            // show message if deleted customer is deleted
                            MessageBox.Show("Customer has been deleted.","Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);		

                        }
                        else
                        {
							// show error that customer has booking and cannot be deleted
                            MessageBox.Show("Customer can not be deleted.\n\n Customer has booking linked to its account.","Invalid operation"
                                            , MessageBoxButton.OK, MessageBoxImage.Information);

                        }

                    }
                }
               
              
            }
            
            catch (ArgumentNullException)  // catch block catch exceptions and show relevant error methods
            {

                MessageBox.Show("Customer does not exist","Customer error", MessageBoxButton.OK, MessageBoxImage.Error);  
            }
            catch (FormatException)
            {

                MessageBox.Show("Please enter a valid number.", "Input error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception cc)  // handles excption if it is not handled in above code and display error message
            {
                
                MessageBox.Show(cc.ToString());
            }
            
        }


		// This event is for Edit customer button and deals with the updation of existing customer
        // it makes save changes button visible and "add" button ued to create new
        // customer hidden. it is useful to perform many operations on same form.
        private void bt_EditCustomer_Click(object sender, RoutedEventArgs e)
        {
            bt_Add_Customer.Visibility = Visibility.Hidden;
            bt_EditCustomer.Visibility = Visibility.Hidden;
            bt_SaveChanges.Visibility = Visibility.Visible;

            tb_CustomerName.Focusable = true;
            tb_CustomerAddress.Focusable = true;
        }



		// This event is for Save Changes button and is used to update the existing customers details
        // it only applies after an existing booking search results are returned using find button
        // it also updates the persistance too
        private void bt_SaveChanges_Click(object sender, RoutedEventArgs e)
        {
           // assign updatd data to relevant properties
            result.pName = tb_CustomerName.Text;
            result.pAddress = tb_CustomerAddress.Text;
             
            UpdateChanges(listOps.pGetCustomersList);   // update/save amended data in database 
            ClearForm();

            MessageBox.Show("Customer " + result.pcustomer_Number + " updated","Confirmation",MessageBoxButton.OK ,MessageBoxImage.Information);
        }

        // 5 //CUSTOMER: This method is used to update the changes made to customers list(peristance lists)
        public void UpdateChanges(List<Customer> customerlist)
        {
            listOps.Save_CustomersList_State();        // this method persist the state of customers list using method from Listoper 
														// which uses booking persistance method to update persist state
        }
    }
}
