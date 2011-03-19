using System;
using NUnit.Framework;
using Expedia;
using Rhino.Mocks;

namespace ExpediaTest
{
	[TestFixture()]
	public class CarTest
	{	
		private Car targetCar;
		private MockRepository mocks;
		
		[SetUp()]
		public void SetUp()
		{
			targetCar = new Car(5);
			mocks = new MockRepository();
		}
		
		[Test()]
		public void TestThatCarInitializes()
		{
			Assert.IsNotNull(targetCar);
		}	
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForFiveDays()
		{
			Assert.AreEqual(50, targetCar.getBasePrice()	);
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForTenDays()
		{
            var target = new Car(10);
			Assert.AreEqual(80, target.getBasePrice());	
		}
		
		[Test()]
		public void TestThatCarHasCorrectBasePriceForSevenDays()
		{
			var target = new Car(7);
			Assert.AreEqual(10*7*.8, target.getBasePrice());
		}
		
		[Test()]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void TestThatCarThrowsOnBadLength()
		{
			new Car(-5);
		}

        [Test()]
        public void TestThatCarDoesGetCarLocationFromDatabase()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();
            string location1 = "The Moon";
            string location2 = "Mars";

            using (mocks.Record())
            {
                mockDatabase.getCarLocation(5);
                LastCall.Return(location1);

                mockDatabase.getCarLocation(7);
                LastCall.Return(location2);
            }

            var target = new Car(10);
            target.Database = mockDatabase;

            string rval = target.getCarLocation(7);
            Assert.AreEqual(rval, location2);

            rval = target.getCarLocation(5);
            Assert.AreEqual(rval, location1);
        }

        [Test()]
        public void TestThatCarDoesGetMileageFromDatabase()
        {
            IDatabase mockDatabase = mocks.Stub<IDatabase>();
            int mileage = 500;

            mockDatabase.Miles = mileage;

            var target = new Car(10);
            target.Database = mockDatabase;

            int rval = target.Mileage;
            Assert.AreEqual(rval, mileage);
        }

        [Test()]
        public void TestThatObjectMotherBMWWorks()
        {
            var result = ObjectMother.BMW();
            Assert.AreEqual(result.Name, "BMW 3 Series");
            Assert.AreEqual(result.getBasePrice(), 80);
        }
	}
}
