USE [CleanArch]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 10/3/2020 9:05:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppRole]    Script Date: 10/3/2020 9:05:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppRole](
	[AppRoleId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AppRole] PRIMARY KEY CLUSTERED 
(
	[AppRoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppRoleClaim]    Script Date: 10/3/2020 9:05:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppRoleClaim](
	[AppRoleClaimId] [int] IDENTITY(1,1) NOT NULL,
	[AppRoleId] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AppRoleClaim] PRIMARY KEY CLUSTERED 
(
	[AppRoleClaimId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppUser]    Script Date: 10/3/2020 9:05:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUser](
	[AppUserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
 CONSTRAINT [PK_AppUser] PRIMARY KEY CLUSTERED 
(
	[AppUserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppUserClaim]    Script Date: 10/3/2020 9:05:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUserClaim](
	[AppClaimId] [int] IDENTITY(1,1) NOT NULL,
	[AppUserId] [int] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AppUserClaim] PRIMARY KEY CLUSTERED 
(
	[AppClaimId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppUserLogin]    Script Date: 10/3/2020 9:05:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUserLogin](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[AppUserId] [int] NOT NULL,
 CONSTRAINT [PK_AppUserLogin] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppUserRole]    Script Date: 10/3/2020 9:05:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUserRole](
	[AppUserId] [int] NOT NULL,
	[AppRoleId] [int] NOT NULL,
 CONSTRAINT [PK_AppUserRole] PRIMARY KEY CLUSTERED 
(
	[AppUserId] ASC,
	[AppRoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AppUserToken]    Script Date: 10/3/2020 9:05:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AppUserToken](
	[AppUserId] [int] NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AppUserToken] PRIMARY KEY CLUSTERED 
(
	[AppUserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DeviceCodes]    Script Date: 10/3/2020 9:05:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DeviceCodes](
	[UserCode] [nvarchar](200) NOT NULL,
	[DeviceCode] [nvarchar](200) NOT NULL,
	[SubjectId] [nvarchar](200) NULL,
	[ClientId] [nvarchar](200) NOT NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[Expiration] [datetime2](7) NOT NULL,
	[Data] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_DeviceCodes] PRIMARY KEY CLUSTERED 
(
	[UserCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PersistedGrants]    Script Date: 10/3/2020 9:05:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PersistedGrants](
	[Key] [nvarchar](200) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[SubjectId] [nvarchar](200) NULL,
	[ClientId] [nvarchar](200) NOT NULL,
	[CreationTime] [datetime2](7) NOT NULL,
	[Expiration] [datetime2](7) NULL,
	[Data] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_PersistedGrants] PRIMARY KEY CLUSTERED 
(
	[Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TodoItems]    Script Date: 10/3/2020 9:05:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TodoItems](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[Created] [datetime2](7) NOT NULL,
	[LastModifiedBy] [int] NOT NULL,
	[LastModified] [datetime2](7) NULL,
	[ListId] [int] NOT NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Note] [nvarchar](max) NULL,
	[Done] [bit] NOT NULL,
	[Reminder] [datetime2](7) NULL,
	[Priority] [int] NOT NULL,
 CONSTRAINT [PK_TodoItems] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TodoLists]    Script Date: 10/3/2020 9:05:03 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TodoLists](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[Created] [datetime2](7) NOT NULL,
	[LastModifiedBy] [int] NOT NULL,
	[LastModified] [datetime2](7) NULL,
	[Title] [nvarchar](200) NOT NULL,
	[Colour] [nvarchar](max) NULL,
 CONSTRAINT [PK_TodoLists] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[AppRoleClaim]  WITH CHECK ADD  CONSTRAINT [FK_AppRoleClaim_AppRole_AppRoleId] FOREIGN KEY([AppRoleId])
REFERENCES [dbo].[AppRole] ([AppRoleId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AppRoleClaim] CHECK CONSTRAINT [FK_AppRoleClaim_AppRole_AppRoleId]
GO
ALTER TABLE [dbo].[AppUserClaim]  WITH CHECK ADD  CONSTRAINT [FK_AppUserClaim_AppUser_AppUserId] FOREIGN KEY([AppUserId])
REFERENCES [dbo].[AppUser] ([AppUserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AppUserClaim] CHECK CONSTRAINT [FK_AppUserClaim_AppUser_AppUserId]
GO
ALTER TABLE [dbo].[AppUserLogin]  WITH CHECK ADD  CONSTRAINT [FK_AppUserLogin_AppUser_AppUserId] FOREIGN KEY([AppUserId])
REFERENCES [dbo].[AppUser] ([AppUserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AppUserLogin] CHECK CONSTRAINT [FK_AppUserLogin_AppUser_AppUserId]
GO
ALTER TABLE [dbo].[AppUserRole]  WITH CHECK ADD  CONSTRAINT [FK_AppUserRole_AppRole_AppRoleId] FOREIGN KEY([AppRoleId])
REFERENCES [dbo].[AppRole] ([AppRoleId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AppUserRole] CHECK CONSTRAINT [FK_AppUserRole_AppRole_AppRoleId]
GO
ALTER TABLE [dbo].[AppUserRole]  WITH CHECK ADD  CONSTRAINT [FK_AppUserRole_AppUser_AppUserId] FOREIGN KEY([AppUserId])
REFERENCES [dbo].[AppUser] ([AppUserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AppUserRole] CHECK CONSTRAINT [FK_AppUserRole_AppUser_AppUserId]
GO
ALTER TABLE [dbo].[AppUserToken]  WITH CHECK ADD  CONSTRAINT [FK_AppUserToken_AppUser_AppUserId] FOREIGN KEY([AppUserId])
REFERENCES [dbo].[AppUser] ([AppUserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AppUserToken] CHECK CONSTRAINT [FK_AppUserToken_AppUser_AppUserId]
GO
ALTER TABLE [dbo].[TodoItems]  WITH CHECK ADD  CONSTRAINT [FK_TodoItems_TodoLists_ListId] FOREIGN KEY([ListId])
REFERENCES [dbo].[TodoLists] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TodoItems] CHECK CONSTRAINT [FK_TodoItems_TodoLists_ListId]
GO
