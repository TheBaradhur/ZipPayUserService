using AutoFixture;
using Dal;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Domain.UnitTests
{
    public class UserServiceTests
    {
        private readonly Fixture _fixture;

        public UserServiceTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public async void ValidateCreationInputsAsync_ShouldReturnValidationError_WhenEmailIsAlreadyUsed()
        {
            // Arrange
            var userRepo = Substitute.For<IUserRepository>();
            var randomEmail = _fixture.Create<string>();
            userRepo.DoesEmailAlreadyExistsAsync(randomEmail).Returns(true);
            var target = new UserService(userRepo);

            // Act
            var actual = await target.ValidateCreationInputsAsync(randomEmail);

            // Assert
            actual.IsValid.Should().BeFalse();
            actual.ErrorCode.Should().Be("EmailExists");
            actual.Error.Should().Be("This email is already used.");
        }

        [Fact]
        public async void ValidateCreationInputsAsync_HappyPath()
        {
            // Arrange
            var userRepo = Substitute.For<IUserRepository>();
            var randomEmail = _fixture.Create<string>();
            userRepo.DoesEmailAlreadyExistsAsync(randomEmail).Returns(false);
            var target = new UserService(userRepo);

            // Act
            var actual = await target.ValidateCreationInputsAsync(randomEmail);

            // Assert
            actual.IsValid.Should().BeTrue();
        }
    }
}