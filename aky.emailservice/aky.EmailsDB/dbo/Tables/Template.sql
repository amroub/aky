CREATE TABLE [dbo].[Template] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (250) NOT NULL,
    [EventCode]      NVARCHAR (500) NOT NULL,
    [TemplatePathId] INT            NOT NULL,
    [SubjectId] INT NOT NULL, 
    CONSTRAINT [PK_Template] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Template_Field] FOREIGN KEY ([TemplatePathId]) REFERENCES [dbo].[Field] ([Id]), 
    CONSTRAINT [FK_Template_Field_SubjectId] FOREIGN KEY ([SubjectId]) REFERENCES [Field]([Id])
);

