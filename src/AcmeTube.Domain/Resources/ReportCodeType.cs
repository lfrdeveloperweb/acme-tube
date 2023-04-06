namespace AcmeTube.Domain.Resources
{
    public enum ReportCodeType
    {
        RequestIsNull = 400000,
        PropertyNameNotFound,
        ResourceNotFound,
        
        PropertyIsNull,
        PropertyInvalidType,
        MandatoryField,
        InvalidSize,
        InvalidDocumentNumber,
        InvalidDocumentType,
        InvalidEmail,
        InvalidPhoneNumber,
        InvalidMonth,
        InvalidYear,
        InvalidChannel,

        DuplicatedDocumentNumber,
        DuplicatedEmail,
        DuplicatedPhoneNumber,
        DuplicatedLogin,

        UserIsLockedOut,
        UserCannotLockItself,
        UserCannotUnlockItself,
        UserSuperAdminCannotBeLockOrUnlock,
        UserIsAlreadyLocked,
        UserIsNotLocked,
        PhoneNumberNotConfirmed,
        EmailNotConfirmed,
        PhoneNumberAlreadyConfirmed,
        EmailAlreadyConfirmed,
        PasswordRequiresUppercaseLetter,
        PasswordRequiresLowercaseLetter,
        PasswordRequiresDigit,
        PasswordRequiresNonAlphanumeric,
        PasswordTooShort,
        ConfirmPasswordNotMatch,
        InvalidToken,
        PasswordMismatch,
        InvalidPassword,
        NewPasswordEqualsCurrentPassword,

        UnsupportedContentType,
        FileExceedsMaximumSizeInMegabytes,

		// Internal Server Errors
		UnexpectedError = 500000,
		LocalFileStorageServiceFailure,
		DropboxFailure,
	}
}
