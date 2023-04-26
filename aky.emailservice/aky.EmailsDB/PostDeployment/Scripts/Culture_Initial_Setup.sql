/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

If Not Exists(Select Id From Culture)
Begin

Insert Into Culture
(CultureName, LanguageCode, CultureCode)
Values('English', 'en', 'en-US')

Insert Into Culture
(CultureName, LanguageCode, CultureCode)
Values('Français', 'fr', 'fr-FR')

Insert Into Culture
(CultureName, LanguageCode, CultureCode)
Values('Español', 'es', 'es-ES')

End