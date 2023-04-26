CREATE TABLE [dbo].[FieldText] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [FieldId]   INT            NOT NULL,
    [Value]     NVARCHAR (MAX) NOT NULL,
    [CultureId] INT            NOT NULL,
    CONSTRAINT [PK_FieldText] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FieldText_Culture] FOREIGN KEY ([CultureId]) REFERENCES [dbo].[Culture] ([Id]),
    CONSTRAINT [FK_FieldText_Field] FOREIGN KEY ([FieldId]) REFERENCES [dbo].[Field] ([Id])
);

