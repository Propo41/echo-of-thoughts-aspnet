using AutoMapper;
using Cefalo.EchoOfThoughts.AppCore.Dtos.User;
using Cefalo.EchoOfThoughts.AppCore.MappingProfiles;
using Cefalo.EchoOfThoughts.Domain.Entities;

namespace Cefalo.EchoOfThoughts.AppCore.UnitTests {
    public class TestConfig {

        public readonly IMapper Mapper;
        public IEnumerable<UserSignUpDto> SignUpDtoLists = new List<UserSignUpDto>();
        public IEnumerable<User> Users = new List<User>();
        public IEnumerable<UserSignInDto> SignInDtoLists = new List<UserSignInDto>();
        public IEnumerable<UserPasswordDto> UserPasswordLists = new List<UserPasswordDto>();

        public TestConfig() {
            var mapperConfiguration = new MapperConfiguration(cfg => {
                cfg.AddProfile<StoryMappingProfile>();
                cfg.AddProfile<UserMappingProfile>();
            });
            Mapper = mapperConfiguration.CreateMapper();

            PopulateDummyData();
        }

        private void PopulateDummyData() {
            Users = new List<User> {
                new()
                {
                    Id = 1,
                    Email = "aliahnaf@gmail.com",
                    FullName = "Ali Ahnaf",
                    ProfilePicture = "https://dicebear.api.com",
                    PasswordHash = "passwordHash",
                    CreatedAt = DateTime.Today,
                    IsDisabled = false,
                    Role = 0,
                    UserName = "ali123",
                    PasswordUpdatedAt = DateTime.Today
                },
                new()
                {
                    Id = 2,
                    Email = "groot@gmail.com",
                    FullName = "Groot Chomski",
                    ProfilePicture = "https://dicebear.api.com",
                    PasswordHash = "passwordHash2",
                    CreatedAt = DateTime.Today,
                    IsDisabled = false,
                    Role = 0,
                    UserName = "groot123",
                    PasswordUpdatedAt = DateTime.Today
                }

            };

            SignUpDtoLists = new List<UserSignUpDto> {
                new() {
                    Email = "ali@gmail.com",
                    UserName = "ali123",
                    Password = "newPassword"
                }

            };

            SignInDtoLists = new List<UserSignInDto> {
                new() {
                    Email = "ali@gmail.com",
                    Password = "newPassword"
                }

            };

            UserPasswordLists = new List<UserPasswordDto> {
                new() {
                    OldPassword = "oldPassword",
                    NewPassword = "newPassword",
                    ConfirmPassword = "newPassword"
                }
            };
        }
    }
}
