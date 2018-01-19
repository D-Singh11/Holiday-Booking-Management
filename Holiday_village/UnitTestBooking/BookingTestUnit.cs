using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessObjects;
using DataLayer;
using System.Collections.Generic;

namespace UnitTestBooking
{
    [TestClass]
    public class BookingUnitTest
    {
     
        /// <summary>
        /// This class is used to test the functionality of the booking class attributes
        /// using test class and test methods.
        /// </summary>
        

        EntitiesOperationsFacade Testing = new EntitiesOperationsFacade();    // instance of EntitiesOperationsFacade class used in test methods


        // Tests if invalid chaletId is provided and it handles its exception
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Chalet id number must be in range of 1-10.")]
        public void InvalidChaletId()
        {
            //Arange
            Booking bookingToTest = new Booking();
            bookingToTest.pArrival_date = new DateTime(2018, 2, 5);
            bookingToTest.pDeparture_date = new DateTime(2018, 2, 7);
            bookingToTest.pChalet_id = 22;

            Customer customer = new Customer();
            customer.pName = "ABC"; customer.pAddress = "123 Street London"; customer.pcustomer_Number = 11;

            Testing.add_customer(customer);

            List<Guest> guestList = new List<Guest>();
            Guest guest1 = new Guest(); String gName1 = "abc"; int gAge1 = 81; string gPassNo1 = "abncvdhg11"; guestList.Add(guest1);
            bookingToTest.pGuest_ist = guestList;

          
        }

        // Tests if invalid departure date is provided and it handles its exception
        [TestMethod]
        [ExpectedException(typeof(Exception), "Departure date can not be before arrival date.")]
        public void InvalidDepartureDate()
        {
           
            Booking bookingToTest = new Booking();
            bookingToTest.pArrival_date = new DateTime(2018, 3, 5);
            bookingToTest.pDeparture_date = new DateTime(2018, 2, 7);
            bookingToTest.pChalet_id = 2;

            Customer customer = new Customer();
            customer.pName = "ABC"; customer.pAddress = "123 Street London"; customer.pcustomer_Number = 11;

            Testing.add_customer(customer);

            List<Guest> guestList = new List<Guest>();
            Guest guest1 = new Guest(); String gName1 = "abc"; int gAge1 = 81; string gPassNo1 = "abncvdhg11"; guestList.Add(guest1);
            bookingToTest.pGuest_ist = guestList;
          
        }

        /// Tests if invalid arrival date( date in past) is provided and it handles its exception
        [TestMethod]
        
        public void InvalidArrivalDateDate()
        {
            //Arange
            Booking bookingToTest = new Booking();
            bookingToTest.pArrival_date = new DateTime(2012, 3, 5);
            bookingToTest.pDeparture_date = new DateTime(2018, 2, 7);
            bookingToTest.pChalet_id = 2;

            Customer customer = new Customer();
            customer.pName = "ABC"; customer.pAddress = "123 Street London"; customer.pcustomer_Number = 11;

            Testing.add_customer(customer);

            List<Guest> guestList = new List<Guest>();
            Guest guest1 = new Guest(); String gName1 = "abc"; int gAge1 = 81; string gPassNo1 = "abncvdhg11"; guestList.Add(guest1);

            
            Assert.IsTrue(bookingToTest.pArrival_date.Date < DateTime.Today.Date);
            ;
        }


        // This test method shows that if no customer is selected
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Booking can not be made without a customer.")]
        public void NoCustomerSelected()
        {
            //Arange
            Booking bookingToTest = new Booking();
            bookingToTest.pArrival_date = new DateTime(2018, 2, 5);
            bookingToTest.pDeparture_date = new DateTime(2018, 2, 7);
            bookingToTest.pChalet_id = 2;

          

            List<Guest> guestList = new List<Guest>();
            Guest guest = new Guest(); String gName1 = "abc"; int gAge1 = 81; string gPassNo1 = "abncvdhg11"; guestList.Add(guest);
            bookingToTest.pGuest_ist = guestList;

            bookingToTest.pCustomerInBooking = null;
            
        }
        
         // Tests method ctest if zero guest is added 
        [TestMethod]        
        public void NoGuestAdded()
        {
            //Arange
            Booking bookingToTest = new Booking();
            bookingToTest.pArrival_date = new DateTime(2018, 1, 5);
            bookingToTest.pDeparture_date = new DateTime(2018, 1, 7);
            bookingToTest.pChalet_id = 2;

            Customer  customer = new Customer();
            customer.pName = "ABC"; customer.pAddress = "123 Street London"; customer.pcustomer_Number = 11;

            Testing.add_customer(customer);
           
            bookingToTest.pGuest_ist = null;

            

        }


        // Test methods handles exception and passes the test if more than 6 guesr are added
        [TestMethod]
      //  [ExpectedException(typeof(ArgumentOutOfRangeException), "Can not add more than 6 guests on booking")]
        public void MoreThan6Guests()
        {
            //Arange
            Booking bookingToTest = new Booking();
            bookingToTest.pArrival_date = new DateTime(2018, 1, 5);
            bookingToTest.pDeparture_date = new DateTime(2018, 1, 7);
            bookingToTest.pChalet_id = 2;

            Customer customer = new Customer();
            customer.pName = "ABC";
            customer.pAddress = "123 Street London";
            customer.pcustomer_Number = 11;

            Testing.add_customer(customer);

           

            List<Guest> guestList = new List<Guest>();

            Guest guest1 = new Guest(); String gName1 = "abc"; int gAge1 = 81; string gPassNo1 = "abncvdhg11"; guestList.Add(guest1);
            Guest guest2 = new Guest(); String gName2 = "abc"; int gAge2 = 82; string gPassNo2 = "abncvdhg12"; guestList.Add(guest2);
            Guest guest3 = new Guest(); String gName3 = "abc"; int gAge3 = 83; string gPassNo3 = "abncvdhg13"; guestList.Add(guest3);
            Guest guest4 = new Guest(); String gName4 = "abc"; int gAge4 = 84; string gPassNo4 = "abncvdhg14"; guestList.Add(guest4);
            Guest guest5 = new Guest(); String gName5 = "abc"; int gAge5 = 85; string gPassNo5 = "abncvdhg15"; guestList.Add(guest5);
            Guest guest6 = new Guest(); String gName6 = "abc"; int gAge6 = 86; string gPassNo6 = "abncvdhg16"; guestList.Add(guest6);
            Guest guest7 = new Guest(); String gName7 = "abc"; int gAge7 = 87; string gPassNo7 = "abncvdhg17"; guestList.Add(guest7);
             bookingToTest.pGuest_ist = guestList;
             

           


        }


        // tests if car hire date is after hire end date
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Car hire start date must be before hire end date and withhin booking dates")]
        public void InvalidCarHireStartDate()
        {
            //Arange
            Booking bookingToTest = new Booking();
            bookingToTest.pArrival_date = new DateTime(2018, 2, 5);
            bookingToTest.pDeparture_date = new DateTime(2018, 2, 7);
            bookingToTest.pChalet_id = 2;

            Customer customer = new Customer();
            customer.pName = "ABC"; customer.pAddress = "123 Street London"; customer.pcustomer_Number = 11;

            Testing.add_customer(customer);

            List<Guest> guestList = new List<Guest>();
            Guest guest1 = new Guest(); String gName1 = "abc"; int gAge1 = 81; string gPassNo1 = "abncvdhg11"; guestList.Add(guest1);
            bookingToTest.pGuest_ist = guestList;

            bookingToTest.pCarHire = true;
            bookingToTest.pHireStartDate = new DateTime(2018, 2, 30);
            bookingToTest.pHireEndDate = new DateTime(2018, 2, 7);
            bookingToTest.pDriver_FullName = "John Smith";

            Assert.IsTrue((bookingToTest.pHireStartDate >= bookingToTest.pArrival_date.Date) && (bookingToTest.pHireStartDate < bookingToTest.pDeparture_date.Date));
            
        }


        // test if car hire end date is before start date
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "Car Hire end date must be after hire start date and withhin booking dates")]
        public void InvalidCarHireEndDate()
        {
            //Arange
            Booking bookingToTest = new Booking();
            bookingToTest.pArrival_date = new DateTime(2018, 2, 5);
            bookingToTest.pDeparture_date = new DateTime(2018, 2, 7);
            bookingToTest.pChalet_id = 2;

            Customer customer = new Customer();
            customer.pName = "ABC"; customer.pAddress = "123 Street London"; customer.pcustomer_Number = 11;

            Testing.add_customer(customer);

            List<Guest> guestList = new List<Guest>();
            Guest guest1 = new Guest(); String gName1 = "abc"; int gAge1 = 81; string gPassNo1 = "abncvdhg11"; guestList.Add(guest1);
            bookingToTest.pGuest_ist = guestList;

            bookingToTest.pCarHire = true;
            bookingToTest.pHireStartDate = new DateTime(2018, 2, 5);
            bookingToTest.pHireEndDate = new DateTime(2018, 1, 7);
            bookingToTest.pDriver_FullName = "John Smith";

            Assert.IsTrue((bookingToTest.pHireEndDate >= bookingToTest.pArrival_date.Date) && (bookingToTest.pHireEndDate <= bookingToTest.pDeparture_date.Date) && (bookingToTest.pHireEndDate >= bookingToTest.pHireStartDate.Date));
            
        }


        // test if car hire start date is out of booking dates range
        [TestMethod]
       // [ExpectedException(typeof(ArgumentOutOfRangeException), "Car Hire end date must be after hire start date and withhin booking dates")]
        public void ValidBookingDetails()
        {
            //Arange
            Booking bookingToTest = new Booking();
            bookingToTest.pArrival_date = new DateTime(2018, 2, 5);
            bookingToTest.pDeparture_date = new DateTime(2018, 2, 7);
            bookingToTest.pChalet_id = 2;

            Customer customer = new Customer();
            customer.pName = "ABC"; customer.pAddress = "123 Street London"; customer.pcustomer_Number = 11;

            Testing.add_customer(customer);

            List<Guest> guestList = new List<Guest>();
            Guest guest1 = new Guest(); String gName1 = "abc"; int gAge1 = 81; string gPassNo1 = "abncvdhg11"; guestList.Add(guest1);
            bookingToTest.pGuest_ist = guestList;

            bookingToTest.pCarHire = true;
            bookingToTest.pHireStartDate = new DateTime(2018, 2, 5);
            bookingToTest.pHireEndDate = new DateTime(2018, 2, 7);
            bookingToTest.pDriver_FullName = "John Smith";

            Assert.IsNotNull(bookingToTest);

        }
    }
}
