using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace BusinessObjects
{

    /// <summary>
   
    /// Guest class holds all the guest properties and validation.
    /// It is also used to validate the input data by the user to assure input data is correct.
    /// </summary>
    public class Guest 
    {
      
        private String name;       		 // private variable to store guest name
        private String passport_Number;  // private variable to store passport number of guest   
        private  int age;				// private variable to store age of guest
       
		
		//************************* 
		//CONSTRUCTOR  of guest class which takes no parameters as arguments
		//*************************
        public Guest()
        {

        }

        	
		// This is another constructor of guest class used to deserialize guest data from file
		// It takes 3 parameters as arguments
		
       [JsonConstructor] 						//this line of code tells constructor to use this constructor when deserialozing guest data
       public Guest(string gname, string gPassportNo, int gAge)
        {
            name = gname;
            passport_Number = gPassportNo;
            age = gAge;
        }
		
		
		
		//************************* 
		// PROPERTIES 
		//*************************//  
		
		// this property is used to get and set  the value of guest name in other classes it returns a string value
        public string pName
        {
            get { return name; }				// return guest name
			
            set 					
            {
                if (String.IsNullOrWhiteSpace(value))  		// if statement value eneterd by user if it is null or white space 
                {
                    throw new ArgumentNullException();		// throw exception if value not string
                }
                name = value;								// otherwise assigns  value to variable "Name"
            }
        }

		
		// this property is used to get and set the value of passport number in other classes it returns a string value
        public string pPassport_number		
        {
            get { return passport_Number; }			// return passport number
            set
            {																  // if statement value eneterd by user if it is null or white space
                if (String.IsNullOrWhiteSpace(value) || value.Length > 10 )  // it also throw exception if length of passport numbeer is over 10 characters
                {
                    throw new ArgumentNullException();		// throw exception if value not string or passport length over 10 characters
                }
                passport_Number = value; 					// otherwise assigns string value to variable "Passport_number"
            }
        }


		
		
		// this property is used to get and set the age of guest in other classes it returns a integer value
        public int pAge
        {
            get { return age; }				// return guest age

            set
            {            
               if (value < 1 || value > 101)		// if statement checks if guest age is less than 1 or over 101
               {
                   throw new ArgumentOutOfRangeException();		// throw exception if age not in range of 1-101
               }
               age = value; 						// otherwise assigns value to variable "Age"

            }

        } 


		
		//************************* 
		// Methods
		//*************************//
		
		// this public method is used to only display guest name on listbox on booking form
		// Without this method combo box was dis[laying object references but not name
        public override string ToString()	// overrise pre written ToString() method
        {
            return ( name);						// return name of guest
        }        
        

        
    }
}
