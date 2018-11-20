using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Vehicles.Tests
{
    public abstract class AbstractFactoryBaseTestData : IEnumerable<object[]>
    {
        public static IVehicleFactory LowGradeVehicleFactory => new LowGradeVehicleFactory();
        public static IVehicleFactory HighGradeVehicleFactory => new HighGradeVehicleFactory();

        protected abstract TheoryData<IVehicleFactory, Type> Data { get; }

        public IEnumerator<object[]> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
