using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace BusinessObjects
{

    /// <summary>
    /// This class holds all the booking and extras(car hire,breakfast and dinner) properties.
    /// It is also used to validate the input data by the user to assure input data is correct.
    /// </summary>


    [DataContract]
    public class Booking 
    {
        [JsonProperty(PropertyName="arrivalDate")]				// used to serialize the arrival date
        private DateTime arrivalDate;							// private variable to store arrival date
		
		
        [JsonProperty(PropertyName = "departureDate")]			// used to serialize the departure date
        private DateTime departureDate;						// private variable to store departure date
		

        [JsonProperty(PropertyName = "bookingRef")]				// used to serialize the booking reference
        private int bookingReference;							// private variable to store booking reference
		
		
        [JsonProperty(PropertyName = "chaletID")]				// used to serialize the chalet id
        private int chaletID;									// private variable to store chalet id
		
		
        [JsonProperty(PropertyName = "customer")]				// used to serialize the customer linked with booing
        private Customer customerInBooking;						// private variable to store the customer deatils linked with the booking
		
      

        [JsonProperty(PropertyName = "guestList")]				// used to serialize the list of guests linked with booking
        private List<Guest> guest_list;							// private variable to store list of guests linked with booking
		
       

        // EXTRAS
         [JsonProperty(PropertyName = "breakFast")]				// used to serialize if breakfast is added on booking or not
        private bool breakfast;									// private variable to store breakfast selection
		

         [JsonProperty(PropertyName = "dinner")]				// used to serialize if dinner is added on booking or not
        private bool dinner;									// private variable to store dinner selection
		

         [JsonProperty(PropertyName = "carHire")]				// used to serialize if car hire is added on booking or not
        private bool car_hire;									// private variable to car hire selection
		

         [JsonProperty(PropertyName = "hireStartDate")]			// used to serialize the car hire start date
         private DateTime carHireStartDate;						// private variable to store start date of car hire
		 

         [JsonProperty(PropertyName = "hireEndDate")]			// used to serialize the car hire end date
         private DateTime carHireEndDate;						// private variable to store end date of car hire
		 

         [JsonProperty(PropertyName = "driverFullName")]		// used to serialize the name of driver for car hire
         private String cardriver_FullName;						// private variable to store the full of driver

       
         private static int autoBookingRef = 0;					// private static variable used to assign auto incremented reference to booking


		 
		 //************************* 
		//CONSTRUCTOR  of Customer class which takes no parameters as arguments
		//*************************
		
        public Booking()
        {
           
        }
		
		
		// This is another constructor of Booking class used to deserialize booking data from file 

        [JsonConstructor] // used only at the time of deserialization
        public Booking(
					   DateTime arrivalDate, DateTime departureDate, int bookingRef, int chaletID,
                       Customer customer, List<Guest> guestList, 
                       bool breakFast, bool dinner, 
                       bool carHire, DateTime hireStartDate, DateTime hireEndDate, string driverFullName
					   )
        
		{
            this.pArrival_date = arrivalDate;				// assigns read value to local variables of the class
            this.pDeparture_date = departureDate;
            this.bookingReference = bookingRef;
          
            this.pChalet_id = chaletID;
            this.customerInBooking = customer;
            this.guest_list = guestList;
            this.breakfast = breakFast;
            this.dinner = dinner;
            this.car_hire = carHire;
			
            if (this.car_hire == true)							// checks if car hire was added at the time of booking made
            {													// if value of car hire variable was true in file
                this.carHireStartDate = hireStartDate;			// then assigns the car hire details to variables
                this.carHireEndDate = hireEndDate;
                this.cardriver_FullName = driverFullName;
            }													// otherwise ignore car hire if block

        }

     

	   //************************* 
	   // PROPERTIES 
       //*************************//  
		
		// this property is used to get and set  the arrival date of booking in other classes and it returns a date
        public DateTime pArrival_date
        {
            get { return arrivalDate; }     // return arrival date
            set
            {
                try							// try block used to check if date was eneterd or not 
                {
                    arrivalDate = value;	// assigns selected date by user to variable
                }
                catch (Exception)			// catch block catches exception if date was not selected
                {
                    throw new Exception();    // throw exception to be cached in booking window class
                }
                 
            }
        }

		
		// this property is used to get and set  the departurel date of booking in other classes and it returns a date
        public DateTime pDeparture_date
        {
            get { return departureDate; }		// return arrival date
            set
            {
                if (value.Date < arrivalDate | value.Date == null )   //  check if departure date is null or before arrival date 
                {
                    throw new Exception();  				// throw exception if departure date is null or before arrival date 
                }
					departureDate = value; 				// otherwise assigns elected value to variable
				}
        }


        // this property is used to get and set the booking refernce number  of booking  and it  returns a integer value
        public int pBooking_reference
        {
            get { return bookingReference; }     // return booking reference
            set { bookingReference = ++autoBookingRef; }	// assigns auto incremented refrence number
        }

       
		// this property is used to get and set the chalet ID 
        public int pChalet_id
        {
            get { return chaletID; }
            set 
            {
                if (value < 1 || value > 10)		// checks if calet Id is in range of 1-10
                {
                    throw new ArgumentException();	// if not throw exception
                }
                chaletID = value; 					// otherwise assigns chalet id to variable
            }
        }
		

		// this property is used to get or set the customer deatils linked with the booking and it  returns a object of customer class
        public Customer pCustomerInBooking
        {
            get { return customerInBooking; }
            set 
            {
                if (value == null)				// checks if customer is selected or not
                {
                    throw new ArgumentNullException(); 		// thrpw exception if not selected
                }
                customerInBooking = value; 				// els store customer details in variable
            }
        }

		
		// this property is used to get or set the guests deatils linked with the booking and it  returns a list of guests
        public List<Guest> pGuest_ist
        {
            get { return guest_list; }
            set 
            {
                //if (guest_list == null)      // checks if list exist or not
                //{
                //    guest_list = new List<Guest>();       // if list doesnot exist it create its instance
                //}
                //if (guest_list.Count > 6  )
                //{
                //    throw new ArgumentOutOfRangeException("Minimum 1 guest should be added and maximum 6");   
                //}
                guest_list = value; 
            }
        }



        // extras properties

		// propety used to set/set the value of breakast extra
        public bool pBreakfast
        {
            get { return breakfast; } 	// return value of breakfast
            set { breakfast = value; }	// assigns value to breakfast variable
        }

		// propety used to set/set the value of dinner extra
        public bool pDinner
        {
            get { return dinner; }		// return value of dinner
            set { dinner = value; }		// assigns value to dinner variable
        }

		// propety used to set/set the value of Car hire  extra
        public bool pCarHire			
        {
            get { return car_hire; }		// return value of car_hire
            set { car_hire = value; }		// assigns value to car_hire variable
        }

		
		// this property is used to get and set the start date car hire extra 
        public DateTime pHireStartDate
        {
            get { return carHireStartDate; }  
            set
            {
                   if (value.Date < pArrival_date)  // check if selected start date is before arrival date
                    {
                        throw new Exception();		// through exception if date is before arrival date
                    }
                    carHireStartDate = value;  		// otherwise assign date to variable
               
            }
        }

		
		// this property is used to get and set the end date car hire extra 
        public DateTime pHireEndDate
        {
            get { return carHireEndDate; }
            set
            {
                if (value.Date < carHireStartDate)			// check if selected end date is before start date
                {
                    throw new ArgumentOutOfRangeException();   // through exception if date is before hire start date
                }
                carHireEndDate = value;			// otherwise assign date to variable
            }
        }

		
		// property to get/set the driver's full name for car hire
        public String pDriver_FullName
        {
            get { return cardriver_FullName; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))  // check if driver name is null or white space
                {
                    throw new ArgumentNullException();  // throw exception if name not in correct format
                }
                cardriver_FullName = value;				// otherwise assign anme to varianle
            }
        }
		
		
		
		// this property is used to get and set the static auto booking Ref number used to assign value to
		// booking reference variable
		// It is also used to restore the state of refernce number at the time of reading booking data from file
		// in Data persistance class in data layer
		public  int pAutoBookingRef
        {
            get { return autoBookingRef; }
            set { autoBookingRef = value; }
        }

  
    }
}
