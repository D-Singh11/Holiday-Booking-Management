using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.IO;


namespace DataLayer
{

	/// <summary>
    /// This class is used to persiist/save the state of application using Serialization
    /// It uses Singleton design pattern so that only one instance of this class is used across
    /// persist the data of application.
    /// It also have instances if Customer and Booking classes to retirve their data from their files.
	/// </summary>
    

    [Serializable]      				// specify that this class should be serialized
    public class DataPersistance
    {
        private static DataPersistance _instance_DataM;			// private instance of this class used to apply singleton pattern


        Customer persistCustRef = new Customer();				// instances of customer class to deal with read/write its data from file
        Booking persistBookRef = new Booking();			// instance of booking class to deal with read/write its data from file
		
		
		//************************* 
		//Priate CONSTRUCTOR  of DataPersistance class which takes no parameters as arguments
		//*************************
        private DataPersistance()
        {

        }
		
		
		//************************* 
		// PROPERTIES 
		//*************************// 

        // singleton starts here
		// This design pattern help use to limit the number of instances of this class to only 1.
		
		// This public and static property is called whenver there is an attempt to create the object
		// of DataPersistance class. If object of this class already exist then it return existing instance
		// otherwise it create its first object and return it.
		
        public static DataPersistance pInstance_DataM			// property to get/set private instance of DataPersistance class
        {
            get
            {
                if (_instance_DataM == null)				// checks if instance is null
                {
                    _instance_DataM = new DataPersistance();	// creates first instance if it was null 
                }
                return _instance_DataM;						// ptherwise return existing instance of this class
            }
        }

        //singleton pattern ends class ends here.


        /// Reading data from the files and storing it to the relevant lists
        /// So that operations can be performed on it.

		
		
        string customerJson = "Customer.html";  // create file name for customer data 
												// can also use Csv or html simply by replacing file name extension
        string bookingfJson = "Booking.html";	// create file name for booking data 

		
		
        

        
		//************************* 
		// Methods
		//*************************//
		
		// READING DATA FROM FILE AND STORES IN LISTS//
		
        // retrieve/read customer data from file
		// This method is used to read data from customer file and store it on list of Customers type.
		// It has a return type of lists used to return the read data to be used by application.
        public List<Customer> retrieve_Customer()
        {
            List<Customer> C_Data = new List<Customer>();			// creates a list of customers to store customers data
			
            try											// try catch to handle exceptions
            {
                if (File.Exists(customerJson))			// if statement checks if customer file exist or not
                {
                    // JSON deserializer used to deserialize data from the file and is stored in the C_Data list
                    C_Data = JsonConvert.DeserializeObject<List<Customer>>(File.ReadAllText(customerJson));
                    
                    if (C_Data.Count > 0)			// check if count of customers is greater then 0
                    {
                        foreach (Customer c in C_Data)		// then iterate through every single customer in list
                        {
                            if (c.pcustomer_Number > persistCustRef.pAutoCustomerRef)  // check if the value of customer number. 
                            {										//in file data is > current the value of auto ref variable in customer class
                                persistCustRef.pAutoCustomerRef = c.pcustomer_Number; // if it is greater than asign that value to variable to restore its state 
                            }

                        }


                    }
                    return C_Data;      // return the customer data list


                }
                return null;          // if file does not exist then return null  
            }
            catch (Exception)		
            {

                throw;  			// throw exception catched from try block
            }

            
        }

		

        //READ BOOKING FROM FILE
		
        // retreieve/read bookings from files
		// This method is used to read data from booking file and store it on list of Booking type.
		// It has a return type of booking list used to return the read data to be used by application.

        public List<Booking> retrieve_Booking()
        {
            List<Booking> B_Data = new List<Booking>();  // creates a list of booking to store booking data

            try
            {
                if (File.Exists(bookingfJson))   // if statement checks if booking file exist or not
                {
                    // JSON deserializer used to deserialize data from the file and is stored in the B_Data list
                    B_Data = JsonConvert.DeserializeObject<List<Booking>>(File.ReadAllText(bookingfJson));

                    if (B_Data.Count > 0)      // check if count of booking is greater then 0
                    {
                        foreach (Booking b in B_Data)    // then iterate through every single booking in list
                        {
                            if (b.pBooking_reference > persistBookRef.pAutoBookingRef) // check if the value of booking reference
                            {										//in file data is > current the value of auto ref variable in booking class
                                persistBookRef.pAutoBookingRef = b.pBooking_reference;
                            }										// if it is greater than asign that value to variable to restore its state

                        }


                    }
                    return B_Data;          // return booking data in lit form


                }
                return null;			// if boking file doesnt exist then return null
            }
            catch (Exception)
            {

                throw;  			 // through exception
            }
        }


        // WRITE DATA TO FILES

        //PERSIST BOOKING STATE
		// This method is used to save the current state of booking objects to file.
		// it takes an parameter of booking list type and use it to write to file.
		// It uses JsonConvert.serializeObject to serialize the data from parameter list .
		// It does not return anything
		
        public void persistBoooking(List<Booking> B_data)
        {
            // this method uses B_data list and serialize all of its data to write in the file
            File.WriteAllText(bookingfJson, JsonConvert.SerializeObject(B_data));  // use Write al method of File
        }																	// to serialize data.

		
		
        // PERSIST CUSTOMER STATE
		// This method is used to save the current state of customer objects to file.
		// it takes an parameter of customer list type and use it to write to file.
		// It uses JsonConvert.serializeObject to serialize the data from parameter list.
		// It does not return anything
		
        public void persistCustomer(List<Customer> C_data)
        {
            // this method uses C_data list and serialize all of its data to write in the file
            File.WriteAllText(customerJson, JsonConvert.SerializeObject(C_data));
        }



    }
   
}
