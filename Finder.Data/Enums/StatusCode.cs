﻿namespace Finder.Data.Enums;

public enum StatusCode
{
    MethodNotAvailable = 0,
    Unauthorized = 1,
    Forbidden = 2,

    UserNotFound = 101,
    RoleNotFound = 102,
    HelperRoleNotFound = 103,
    UserRoleNotFound = 104,
    DirectoryNotFound = 105,
    FileNotFound = 106,
    HelpRequestNotFound = 107,
    ImageNotFound = 108,

    QueryResultError = 201,

    EmailSendingError = 301,
    ImageProcessingError = 302,

    UserHasNoRole = 401,

    FileHasAnUnacceptableFormat = 501,

    RoleCannotBeUpdated = 601,
    RoleCannotBeDeleted = 602,
    UserRoleCannotBeChanged = 603,
    TagCannotBeEmpty = 604,

    FirstNameTooLong = 701,
    LastNameTooLong = 702,
    FileIsTooLarge = 703,
    EmailTooLong = 704,
    AddressLine1TooLong = 705,
    AddressLine2TooLong = 706,
    CityTooLong = 707,
    StateTooLong = 708,
    PostalCodeTooLong = 709,
    CountryTooLong = 710,
    InstagramTooLong = 711,
    LinkedInTooLong = 712,
    PhoneNumberTooLong = 713,
    SkypeTooLong = 714,
    TelegramTooLong = 715,
    OtherTooLong = 716,
    TitleTooLong = 717,
    DescriptionTooLong = 718,
    TagTooLong = 719,
    EndpointTooLong = 720,
    AuthTooLong = 721,
    P256dhTooLong = 722,

    IncorrectPassword = 801,
    PasswordsDoNotMatch = 802,
    OldPasswordIncorrect = 803,

    UserAlreadyExists = 901,
    RoleAlreadyExists = 902,
    EmailAlreadyExists = 903,

    RoleTypeRequired = 1001,
    RoleIdRequired = 1002,
    UserIdRequired = 1003,
    LastNameRequired = 1004,
    FirstNameRequired = 1005,
    InvitationTokenRequired = 1006,
    PasswordRequired = 1007,
    EmailRequired = 1008,
    RoleNameRequired = 1009,
    VerificationCodeRequired = 1010,
    ConfirmPasswordRequired = 1011,
    AddressLine1Required = 1012,
    CityRequired = 1013,
    StateRequired = 1014,
    PostalCodeRequired = 1015,
    CountryRequired = 1016,
    ImagesRequired = 1017,
    DescriptionRequired = 1018,
    TitleRequired = 1019,
    AuthRequired = 1020,
    P256dhRequired = 1021,
    KeysRequired = 1022,
    EndpointRequired = 1023,
    FilterTitlesRequired = 1024,
    FilterTagsRequired = 1025,

    PasswordLengthExceeded = 1101,

    PasswordMustHaveAtLeast8Characters = 1201,
    PasswordMustHaveNotMoreThan32Characters = 1202,
    PasswordMustHaveAtLeastOneUppercaseLetter = 1203,
    PasswordMustHaveAtLeastOneLowercaseLetter = 1204,
    PasswordMustHaveAtLeastOneDigit = 1205,

    InvalidRoleType = 1301,
    InvalidSortByProperty = 1302,
    InvalidExpandProperty = 1303,
    InvalidEmailFormat = 1304,
    InvalidEmailModel = 1305,
    InvalidVerificationCode = 1306,
    InvalidFile = 1307,
    InvalidPhoneNumber = 1308,
    InvalidNotificationSettings = 1309,

    FirstNameShouldNotContainWhiteSpace = 1401,
    LastNameShouldNotContainWhiteSpace = 1402,

    HelpRequestRemoved = 1501
}