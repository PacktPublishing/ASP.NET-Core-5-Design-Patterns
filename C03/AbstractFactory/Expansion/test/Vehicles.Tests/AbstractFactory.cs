using System;
using Vehicles.Models;
using Xunit;

namespace Vehicles.Tests
{
    public class AbstractFactory
    {
        // Arrange
        public static IVehicleFactory LowGradeVehicleFactory => new LowGradeVehicleFactory();
        public static IVehicleFactory HighGradeVehicleFactory => new HighGradeVehicleFactory();
        public static IVehicleFactory MiddleEndVehicleFactory => new MiddleEndVehicleFactory();

        public static TheoryData<IVehicleFactory, Type> CarData => new TheoryData<IVehicleFactory, Type>{
            { LowGradeVehicleFactory, typeof(LowGradeCar) },
            { HighGradeVehicleFactory, typeof(HighGradeCar) },
            { MiddleEndVehicleFactory, typeof(MiddleGradeCar) },
        };
        public static TheoryData<IVehicleFactory, Type> BikeData => new TheoryData<IVehicleFactory, Type>{
            { LowGradeVehicleFactory, typeof(LowGradeBike) },
            { HighGradeVehicleFactory, typeof(HighGradeBike) },
            { MiddleEndVehicleFactory, typeof(MiddleGradeBike) },
        };

        [Theory]
        [MemberData(nameof(CarData))]
        public void Should_create_a_Car_of_the_specified_type(IVehicleFactory vehicleFactory, Type expectedCarType)
        {
            // Act
            var result = vehicleFactory.CreateCar();

            // Assert
            Assert.IsType(expectedCarType, result);
        }

        [Theory]
        [MemberData(nameof(BikeData))]
        public void Should_create_a_Bike_of_the_specified_type(IVehicleFactory vehicleFactory, Type expectedBikeType)
        {
            // Act
            var result = vehicleFactory.CreateBike();

            // Assert
            Assert.IsType(expectedBikeType, result);
        }
    }
}
