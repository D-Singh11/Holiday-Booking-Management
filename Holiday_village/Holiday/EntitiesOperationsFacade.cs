using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
	
	/// <summary>
    /// This class allow all layers to communicate with each other.
    /// It uses facade design beacuse it hides the complexity of all three Customer,Booking and Guest classes from user.
    /// It is resposible for performing required operations on core classes.
    /// It has private lists of those classes and uses their methods and proprties to allow other layers to access them.
    /// It has an instance of DataPersistance class to persist the data of application.
	/// </summary>
    

    public class EntitiesOperationsFacade
    {
	
        DataPersistance DataPersist = DataPersistance.pInstance_DataM;
		
		// facade starts here : This class uses private lists and actual instances of classes within its methos
		// to access properties and methods of core classes to perform operations on data.
		
        private List<Customer> AllCustomer_List;	 // create private list of customers to store current customers data
        private List<Booking> AllBookings_List;		 // create private list of booking to store current bookings data
        private List<Guest> Guest_List = new List<Guest>(); // create private list of guests to store current guests data

        



		//************************* 
		//CONSTRUCTOR  of class 
		//*************************
        //////////CONSTRUCTOR///////////////////

        public EntitiesOperationsFacade()
        {
            AllCustomer_List = DataPersist.retrieve_Customer();   // store customers data retrived from Customer json file 
            AllBookings_List = DataPersist.retrieve_Booking();    // store booking data retrived from Booking json file 
        }
       


	   /////////////////// CUSTOMER operations/////////////////////

        //************************* 
		// PROPERTIES 
		//*************************// 

		// property for customer list
        public List<Customer> pC_ustomer_List
        {
            get { return AllCustomer_List; }
            set { AllCustomer_List = DataPersist.retrieve_Customer();} // read data from file and assign to list
        }

		
		// property for customer list
		 public List<Booking> pAllBooking_List
        {
            get { return AllBookings_List; }
            set { AllBookings_List = DataPersist.retrieve_Booking();} //read data from file and assign to list
        }
		
		
		// property for customer list
		 public List<Guest> pGuest_List
        {
            get { return Guest_List; }
            set { Guest_List = value; }
        }
		
		
		
		
		//************************* 
		// Methods
		//*************************//
		
        // 1 // CUSTOMER : Find method to find customer from list
			// return customer object

        public Customer findCustomer(int custRef)
        {

            foreach (Customer existingCustomer in AllCustomer_List)  // iterate through customers list
            {
                if (existingCustomer.pcustomer_Number == custRef)	// check customer ref number enterd by user with existing customers
                {
                    return existingCustomer;			// if numbers maych then return customer linked to that number
                }

            }
            return null;					// else return null
        }

        // 2 //CUSTOMER : Method to delete customer customer from list and update its state
		// returns boolean value
        public bool  delete_Customer(int customerRef)
        {

            Customer customer = this.findCustomer(customerRef);   // find cstomer using find method 
           
            foreach (Booking existingBooking in AllBookings_List)
            {
                if (existingBooking.pCustomerInBooking.pcustomer_Number == customerRef || customer == null)   // can not delete customer if has a booking
                {
                    return false;		//return false if customer is null or has a booking
                }
             
            }
           
             AllCustomer_List.Remove(customer);      // remove customer from list
             Save_CustomersList_State();             // save changes made to customer list in database
             return true;							// return true if customer deleted from list

            
           

            
           
        }

        // 3 //CUSTOMER: Add new customer used to add new customer to list

        public  void add_customer(Customer newCustomer)
        {
        
            if (AllCustomer_List == null)      // checks if list exist or not
            {
                AllCustomer_List = new List<Customer>();       // if list doesnot exist it create its instance
            }                                               // other wise adds customer to existing list
            AllCustomer_List.Add(newCustomer);     //  calls pre-wriiten "Add" method of "List" class to new customer
            Save_CustomersList_State();             // update current state of customer list in database
        }


        // 4 //CUSTOMER: returns list of all customers 

		// read only property used to read all customers details
		// return  list of customers
        public List<Customer> pGetCustomersList
        {
            get { return AllCustomer_List; }                 // get block returns the guest list

        }



        // 5 //CUSTOMER: this method is used to save the state of customers list 
			// it uses a method called "persistCustomer" of DataPersistance class and  has no retur type
        public void Save_CustomersList_State()
        {           
            DataPersist.persistCustomer(AllCustomer_List);			
        }





        
        //////////////////////// BOOKING OPERATIONS//////////////////////////////////////////////////////////
        


        // 1 //BOOKING : This method adds new booking to Booking list

        public void add_Booking(Booking newbooking)
        {
            if (AllBookings_List == null)      // checks if list exist or not
            {
                AllBookings_List = new List<Booking>();       // if list doesnot exist it create its instance
            }                                               // other wise adds booking to existing list
            AllBookings_List.Add(newbooking);
            Save_Bookings_State();           // update the current state of  booking list in database
        }



        // 2  // BOOKING: Finding booking and return booking data

        public Booking findBooking(int bookinref)
        {
            foreach (Booking existingBooking in AllBookings_List)
            {
                //if an object has matching reference number it is added to the result's list
                if (existingBooking.pBooking_reference == bookinref)
                {
                    return existingBooking;
                }
            }
            //we return filtered objects
            return null;
        }


        // 3  //BOOKING : This method delete booking from list and return true if deleted
		      // otherwise return false.

        public Boolean delete_Booking(int booking_ref)
        {
            foreach (Booking existingBooking in AllBookings_List)
            {
                if (existingBooking.pBooking_reference == booking_ref)
                {
                    AllBookings_List.Remove(existingBooking);  // remove booking from list
                    Save_Bookings_State();                   // save changes made to booking list in database
                    return true;
                }
            }
            return false;
            

        }



        // 4  //BOOKING: returns list of all bookings

        public List<Booking> pGetBookingList
        {
            get { return AllBookings_List; }                   // get block returns the guest list

        }


        // 5 //BOOKING: this method is used to save the state of bookings list

        public void Save_Bookings_State()
        {
            DataPersist.persistBoooking(AllBookings_List);
        }

		// This method is used  to make sure only maximum of 6 and minumum of 1 guest gets added tp booking
        public void guestCounInRange(List<Guest> guestList)
        {
            if (guestList == null || guestList.Count > 6 || guestList.Count < 1)
	        {
                throw new ArgumentOutOfRangeException();  // throw exception if condition false
	        }
        }




		// **************GUEST METHODS**************
		// Guest doest not hace persistance method because it gets its state saved through booking class.


        // 1 //Guest :Add new Guest to Guest list
        public void add_Guest(Guest newGuest)
        {
            if (Guest_List == null)      // checks if list exist or not
            {
                Guest_List = new List<Guest>();       // if list doesnot exist it create its instance
            }                                               // other wise adds Guest to existing list
            Guest_List.Add(newGuest);
          
        }



        // 2  // Guest: Finding Guest and return Guest data
        public Guest findGuest(string guestPassportNo)
        {
            foreach (Guest g in Guest_List)
            {
                //if an object has matching reference number it is added to the result's list
                if (g.pPassport_number == guestPassportNo)
                {
                    return g;			// return matched guest details
                }
            }
            
            return null;
        }


        // 3  //Guest : This method is used to remove guest from guest list	
			  // it takes passport number string as parameter and matches it with existing records
			  // if match found delete guest oand return true oterwise return false.
			  
        public Boolean delete_Guest(string guestPassportNo)
        {
            foreach (Guest existingGuest in Guest_List)
            {
                if (existingGuest.pPassport_number == guestPassportNo)
                {
                    Guest_List.Remove(existingGuest);

                    return true;
                }
            }
            return false;

        }

      



        // 4  //Guest: returns list of all Guests
        public List<Guest> pShowGuestList
        {
            get { return Guest_List; }                   // get block returns the guest list

        }


    

    }
    
}
