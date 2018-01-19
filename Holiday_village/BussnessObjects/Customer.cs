using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BusinessObjects
{

    /// <summary>
    
    /// Customer class used to  all the customer properties.
    /// It is also used to validate the input data by the user to assure input data is correct.
    /// </summary>
    [DataContract]
     public class Customer 
    {
		
        [JsonProperty(PropertyName="customerRef")]  // used to serialize the Customer reference
        private int CustomerRef;					// private variable to store customer reference
        
        [JsonProperty(PropertyName = "Cname")]		// used to serialize the Cname
        private String name;						// private variable to store customer name
        
        [JsonProperty(PropertyName = "Caddress")]	// used to serialize the Customer address
         private String addres;						// private variable to store customer address

        private static int autoCustomerRef = 0;		// private static variable used to assign auto incremented reference to customer
		
		 private static List<Booking> bookings_list = new List<Booking>();

       
       

		//************************* 
		//CONSTRUCTOR  of Customer class which takes no parameters as arguments
		//*************************
        public Customer()
        {
            
        }
		
		
		// This is another constructor of Customer class used to deserialize Customer data from file 
		// It takes 3 parameters as arguments

        [JsonConstructor] 					// this line of code tells constructor to use this constructor when deserialozing customer data
        public Customer(int customerRef, string Cname, string Caddress)
        {
            
            CustomerRef = customerRef;
            name = Cname;
            addres = Caddress;
                
        }


		
		//************************* 
		// PROPERTIES 
		//*************************//  
		
		// this property is used to get and set  the customer name in other classes it returns a string value
        public string pName
        {
            get { return name; }							// return customer name
            set
            {
                if (String.IsNullOrWhiteSpace(value))		// if statement value eneterd by user if it is null or white space
                {
                    throw new ArgumentNullException();      // throw exception if value not string or white space
                }
                else
                {
                    name = value;						// otherwise assigns  value to variable "Name"
                }
                
            }
        }

		
		// this property is used to get and set  the customer address in other classes it returns a string value
        public string pAddress
        {
            get { return addres; }							// return customer address
            set
            {
                if (String.IsNullOrWhiteSpace(value))		// if statement value eneterd by user if it is null or white space
                {
                    throw new ArgumentNullException();		// throw exception if value not string or white space
                }
                else
                {
                    addres = value;							// otherwise assigns  value to variable "Address"
                }
                
            }
        }


		
		// this property is used to get and set the customer refernce number  of customer  and it  returns a integer value
        public int pcustomer_Number
        {
            get { return CustomerRef; }			// returns customer reference number
            set
            {
                if (!string.IsNullOrWhiteSpace(pName) && !string.IsNullOrWhiteSpace(pAddress))  // if statement checks if name and address of customer
                {																				// was in correct format
                    CustomerRef = ++autoCustomerRef;  										// and assigns auto incremented refrence number
                }
                else															
                {
                   // refFactory.autoCustomerRef--;
                    throw new Exception();										// otherwise throw exception
                }
                
            }
        }

       

        public List<Booking> pBookingsList
        {

            get
            {
                return bookings_list;
            }
            set
            {
               // bookings_list = new List<Booking>();
                bookings_list = value;
            }
        }

        public void add(Booking newbooking)                 // add method which takes one argument of type guest
        {                                                   //  and add instance of student class on " list 
            bookings_list.Add(newbooking);                   //  calls pre-wriiten "Add" method of "List" class  
        }

        public List<Booking> pShowGuestList
        {
            get { return bookings_list; }                   // get block returns the guest list

        }
		
		
		// this property is used to get and set the static auto customer Ref number used to assign value to
		// customer reference variable
		// It is also used to restore the state of refernce number at the time of reading customer data from file
		// in Data persistance class in data layer
		public int pAutoCustomerRef
        {
            get { return autoCustomerRef; }				// return  value
            set { autoCustomerRef = value; }			// assign value to variable
        }
		
		
		
		//************************* 
		// Methods
		//*************************//
		
		
        // this method is used to display customer ref and name on customer combo box on booking form
		// Without this method combo box was displaying object references but not refrence and name
        public override string ToString()
        {
            return (CustomerRef +"  "+ name);		// return customer reference and name to display in combobox
        }
       

        //public AutogeneratedRefrence pRefFactory
        //{
        //    get { return refFactory; }
        //    set { refFactory = value; }
            
       // }
    }
}
