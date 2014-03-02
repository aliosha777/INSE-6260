using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Tests
{
    using Ploeh.AutoFixture;
    using Ploeh.AutoFixture.Kernel;

    public class BankingCustomization : CompositeCustomization
    {
    }

    public class AccountCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customizations.Add(new AccountBuilder());
        }
    }

    public class AccountBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            return null;
        }
    }
}
