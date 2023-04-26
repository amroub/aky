CREATE TABLE [dbo].[Culture] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [CultureName]  NVARCHAR (100) NOT NULL,
    [LanguageCode] NVARCHAR (10)  NOT NULL,
    [CultureCode]  NVARCHAR (10)  NOT NULL,
    CONSTRAINT [PK_Culture] PRIMARY KEY CLUSTERED ([Id] ASC)
);

