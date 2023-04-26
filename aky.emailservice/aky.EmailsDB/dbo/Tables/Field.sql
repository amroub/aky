CREATE TABLE [dbo].[Field] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_Field] PRIMARY KEY CLUSTERED ([Id] ASC)
);

