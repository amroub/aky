

IF NOT EXISTS (
		SELECT Id
		FROM Template
		WHERE EventCode = 'ForgotPasswordEvent'
		)
BEGIN
	DECLARE @TemplatePathId INT
	DECLARE @SubjectId INT

	INSERT INTO Field (NAME)
	VALUES ('TemplatePathId')

	SELECT @TemplatePathId = @@IDENTITY

	INSERT INTO Field (NAME)
	VALUES ('SubjectId')

	SELECT @SubjectId = @@IDENTITY

	-- Add a new entry into "Template" table for reset password email
	INSERT INTO Template (
		[Name]
		,EventCode
		,TemplatePathId
		,SubjectId
		)
	VALUES (
		'Reset password email template'
		,'ForgotPasswordEvent'
		,@TemplatePathId
		,@SubjectId
		)

	-- Add language specific record "FieldText" table for template created for reset password email
	-- english locale for template path
	INSERT INTO FieldText (
		FieldId
		,Value
		,CultureId
		)
	VALUES (
		@TemplatePathId
		,'emailtemplates/reset-password/aky.email.template.forgot.password.ENG.html'
		,1
		)

	-- french locale for template path
	INSERT INTO FieldText (
		FieldId
		,Value
		,CultureId
		)
	VALUES (
		@TemplatePathId
		,'emailtemplates/reset-password/aky.email.template.forgot.password.FRA.html'
		,2
		)

	-- spanish locale for template path
	INSERT INTO FieldText (
		FieldId
		,Value
		,CultureId
		)
	VALUES (
		@TemplatePathId
		,'emailtemplates/reset-password/aky.email.template.forgot.password.ESP.html'
		,3
		)

	-- english locale for email subject
	INSERT INTO FieldText (
		FieldId
		,Value
		,CultureId
		)
	VALUES (
		@SubjectId
		,'Forgot your password'
		,1
		)

	-- french locale for email subject
	INSERT INTO FieldText (
		FieldId
		,Value
		,CultureId
		)
	VALUES (
		@SubjectId
		,'Mot de passe oublié'
		,2
		)

	-- spanish locale for email subject
	INSERT INTO FieldText (
		FieldId
		,Value
		,CultureId
		)
	VALUES (
		@SubjectId
		,'¿Olvidaste tu contraseña?'
		,3
		)
END
GO

