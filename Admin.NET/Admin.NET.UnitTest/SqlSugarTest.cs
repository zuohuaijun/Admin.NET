using Admin.NET.Application.Serice;
using Admin.NET.Core;
using Furion;
using Xunit;

namespace Admin.NET.UnitTest
{
    public class SqlSugarTest
    {
        private readonly TestService testService;

        public SqlSugarTest()
        {
            testService = App.GetService<TestService>();
        }

        [Fact]
        public async void Test1()
        {
            var page = await testService.GetTestList();
            Assert.True(page.Count > 0);
        }

        [Fact]
        public async void Test2()
        {
            var userManager = App.GetService<IUserManager>();
            var user = await userManager.CheckUserAsync(0);
            Assert.NotNull(user);
        }
    }
}