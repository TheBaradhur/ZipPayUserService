using System.Collections.Generic;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;
using ZipPay.User.Infrastructure;
using ZipPay.User.Infrastructure.Models;

namespace ZipPay.User.Domain.UnitTests
{
    public class AccountServiceTests
    {
        private readonly Fixture _fixture;

        private readonly IAccountRepository _accountRepo;

        private readonly IUserService _userService;

        private const decimal validCreditAmount = 500m;

        public AccountServiceTests()
        {
            _fixture = new Fixture();

            _accountRepo = Substitute.For<IAccountRepository>();
            _userService = Substitute.For<IUserService>();
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(1000.01)]
        [InlineData(9.99999999)]
        public async void ValidateCreationInputsAsync_ShouldReturnValidationError_WhenCreditAmountIsInvalid(decimal creditAmount)
        {
            // Arrange
            var randomId = _fixture.Create<int>();

            var target = new AccountService(_accountRepo, _userService);

            // Act
            var actual = await target.ValidateCreationInputsAsync(randomId, creditAmount);

            // Assert
            actual.IsValid.Should().BeFalse();
            actual.ErrorCode.Should().Be("CreditAmountInvalid");
            actual.Error.Should().Be("The credit can only be between 10 and 1000 AUD (included).");
        }

        [Fact]
        public async void ValidateCreationInputsAsync_ShouldReturnValidationError_WhenUserDoesNotExist()
        {
            // Arrange
            var randomId = _fixture.Create<int>();

            _userService.GetUserByIdAsync(randomId).ReturnsNull();

            var target = new AccountService(_accountRepo, _userService);

            // Act
            var actual = await target.ValidateCreationInputsAsync(randomId, validCreditAmount);

            // Assert
            actual.IsValid.Should().BeFalse();
            actual.ErrorCode.Should().Be("UserNotFound");
            actual.Error.Should().Be($"The user with id {randomId} does not exist.");
        }

        [Fact]
        public async void ValidateCreationInputsAsync_ShouldReturnValidationError_WhenUserAllowanceIsBelowTheLimit()
        {
            // Arrange
            var randomId = _fixture.Create<int>();
            var user = _fixture
                .Build<UserEntity>()
                .With(x => x.MonthlySalary, 1000m)
                .With(x => x.MonthlyExpenses, 500m)
                .Create();

            _userService.GetUserByIdAsync(randomId).Returns(user);

            var target = new AccountService(_accountRepo, _userService);

            // Act
            var actual = await target.ValidateCreationInputsAsync(randomId, validCreditAmount);

            // Assert
            actual.IsValid.Should().BeFalse();
            actual.ErrorCode.Should().Be("NotEnoughCreditAllowance");
            actual.Error.Should().StartWith("The user allowance should be at least");
        }

        [Fact]
        public async void ValidateCreationInputsAsync_ShouldReturnValidationError_WhenUserAllowanceIncludingExistingAccountsIsBelowTheLimit()
        {
            // Arrange
            var randomId = _fixture.Create<int>();
            var user = _fixture
                .Build<UserEntity>()
                .With(x => x.Id, randomId)
                .With(x => x.MonthlySalary, 5000m)
                .With(x => x.MonthlyExpenses, 3500m)
                .Create();

            var account = _fixture
                .Build<AccountEntity>()
                .With(x => x.CurrentBalance, 1000)
                .Create();

            _userService.GetUserByIdAsync(randomId).Returns(user);
            _accountRepo.GetByUserIdAsync(randomId).Returns(new List<AccountEntity> { account });

            var target = new AccountService(_accountRepo, _userService);

            // Act
            var actual = await target.ValidateCreationInputsAsync(randomId, AccountService.MaximumCreditValue);

            // Assert
            actual.IsValid.Should().BeFalse();
            actual.ErrorCode.Should().Be("NotEnoughCreditAllowance");
            actual.Error.Should().StartWith("The user already has open accounts for a total of");
        }

        [Fact]
        public async void ValidateCreationInputsAsync_HappyPath()
        {
            // Arrange
            var randomId = _fixture.Create<int>();
            var user = _fixture
                .Build<UserEntity>()
                .With(x => x.Id, randomId)
                .With(x => x.MonthlySalary, 5000m)
                .With(x => x.MonthlyExpenses, 3500m)
                .Create();

            var account = _fixture
                .Build<AccountEntity>()
                .With(x => x.CurrentBalance, 500)
                .Create();

            _userService.GetUserByIdAsync(randomId).Returns(user);
            _accountRepo.GetByUserIdAsync(randomId).Returns(new List<AccountEntity> { account });

            var target = new AccountService(_accountRepo, _userService);

            // Act
            var actual = await target.ValidateCreationInputsAsync(randomId, AccountService.MaximumCreditValue);

            // Assert
            actual.IsValid.Should().BeTrue();
        }
    }
}