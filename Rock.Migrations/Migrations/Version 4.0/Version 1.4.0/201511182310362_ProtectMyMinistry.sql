    UPDATE [AttributeValue] SET [Value] = 'False'
    WHERE [Guid] = '40A59878-1417-4811-BC5C-6C39729C1A98'

    DECLARE @JurisdictionCodeDefinedTypeId int = ( SELECT TOP 1 [Id] FROM [DefinedType] WHERE [Guid] = '2F8821E8-05B9-4CD5-9FA4-303662AAC85D' )
    UPDATE [AttributeQualifier] SET [Value] = CAST( @JurisdictionCodeDefinedTypeId AS varchar )
    WHERE [Guid] = '9599BD09-1B79-4E00-8FC6-D9DC19010E56'

    DECLARE @AttributeId int = ( SELECT TOP 1 [Id] FROM [Attribute] WHERE [Guid] = 'A4CB9461-D77F-40E0-8DFF-C7838D78F2EC' )
    DECLARE @DefinedTypeId int = ( SELECT TOP 1 [Id] FROM [DefinedType] WHERE [Guid] = 'BC2FDF9A-93B8-4325-8DE9-2F7B1943BFDF' )
    DELETE [AttributeQualifier] 
    WHERE [AttributeId] = @AttributeId
    AND [Guid] NOT IN ( 'D03418DA-BC29-47C3-AA36-1051841C62F9', 'CE080F26-ACDA-45FB-BE30-0D88688FF99B', '101C2F35-28F8-4E20-B1E2-812D8F784E8F' )

    UPDATE [AttributeQualifier]
    SET [Value] = CAST( @DefinedTypeId as varchar) 
    WHERE [AttributeId] = @AttributeId
    AND [Guid] = 'D03418DA-BC29-47C3-AA36-1051841C62F9'

    UPDATE [AttributeValue] 
    SET [Value] = CASE WHEN [Value] = 'Basic' THEN 'B091BE26-1EEA-4601-A65A-A3A75CDD7506' ELSE 'C542EFC7-1D22-4DBD-AF09-5C583FCD4FEF' END
    WHERE [AttributeId] = @AttributeId

    DECLARE @BackgroundCheckWorkflowTypeId int = ( SELECT TOP 1 [Id] FROM [WorkflowType] WHERE [Guid] = '16D12EF7-C546-4039-9036-B73D118EDC90' )
    DECLARE @WorkflowEntityTypeId int = ( SELECT TOP 1 [Id] FROM [EntityType] WHERE [Name] = 'Rock.Model.Workflow' )

    DECLARE @PopularState varchar(50)
    ;WITH CTE AS 
    ( 
	    SELECT TOP 200 [State] FROM [Location] 
    ),
    CTE2 AS 
    ( 
	    SELECT [State], COUNT(*) AS [Frequency]
	    FROM CTE
	    GROUP BY [State]
    )

    SELECT TOP 1 @PopularState = [State]
    FROM CTE2
	WHERE [State] IS NOT NULL
    ORDER BY [Frequency] DESC

    IF @PopularState IS NOT NULL
    BEGIN

        -- Update the PLUS package state
        SET @AttributeId = ( SELECT TOP 1 [Id] FROM [Attribute] WHERE [Guid] = '17093E08-F287-4A77-87B7-5FFA2337A8B7' )
        DECLARE @EntityId int = ( SELECT TOP 1 [Id] FROM [DefinedValue] WHERE [Guid] = 'C542EFC7-1D22-4DBD-AF09-5C583FCD4FEF' )
        UPDATE [AttributeValue] 
        SET [Value] = @PopularState
        WHERE [AttributeId] = @AttributeId
        AND [EntityId] = @EntityId

        -- Update the MVR state
        SET @AttributeId = ( SELECT TOP 1 [Id] FROM [Attribute] WHERE [Guid] = '1169005D-662B-4380-9FFD-BD6177037329' )
        SET @EntityId = ( SELECT TOP 1 [Id] FROM [DefinedValue] WHERE [Guid] = 'D27F591E-0016-4924-BC8D-F3F488DF3F8C' )
        DECLARE @DefinedValueGuid uniqueidentifier = ( 
            SELECT TOP 1 [Guid] 
            FROM [DefinedValue] 
            WHERE [DefinedTypeId] = @JurisdictionCodeDefinedTypeId
           AND [Value] LIKE @PopularState + '%'
        )

        IF @DefinedValueGuid IS NOT NULL
        BEGIN
            UPDATE [AttributeValue] 
            SET [Value] = CAST( @DefinedValueGuid as varchar(60) )
            WHERE [AttributeId] = @AttributeId
            AND [EntityId] = @EntityId
        END

    END

    IF @BackgroundCheckWorkflowTypeId IS NOT NULL
    AND @WorkflowEntityTypeId IS NOT NULL
    BEGIN

	    -- Get the workflow attribute that stores the person
	    DECLARE @PersonAttributeId int = ( 
		    SELECT TOP 1 [Id] 
		    FROM [Attribute]
		    WHERE [EntityTypeId] = @WorkflowEntityTypeId
		    AND [EntityTypeQualifierValue] = CAST( @BackgroundCheckWorkflowTypeId as varchar )
		    AND [Key] = 'Person' 
	    )

	    -- If found...
	    IF @PersonAttributeId IS NOT NULL
	    BEGIN

		    -- Insert a background check for each person who has a workflow
		    INSERT INTO [BackgroundCheck] ( [PersonAliasId], [WorkflowId], [RequestDate], [Guid] )
		    SELECT 
			    PA.[Id],
			    W.[Id],
			    W.[CreatedDateTime],
                NEWID()
		    FROM [WorkFlow] W
		    INNER JOIN [AttributeValue] AV
			    ON AV.[AttributeId] = @PersonAttributeId
			    AND AV.[EntityId] = W.[Id]
		    INNER JOIN [PersonAlias] PA
			    ON PA.[Guid] = AV.[Value]
		    WHERE W.[WorkflowTypeId] = @BackgroundCheckWorkflowTypeId
		    AND AV.[Value] IS NOT NULL
		    AND AV.[Value] <> ''

		    -- Get the attribute that stores the status
		    DECLARE @ReportStatusAttributeId int = (
			    SELECT TOP 1 [Id] 
			    FROM [Attribute]
			    WHERE [EntityTypeId] = @WorkflowEntityTypeId
			    AND [EntityTypeQualifierValue] = CAST( @BackgroundCheckWorkflowTypeId as varchar )
			    AND [Key] = 'ReportStatus' 
		    )
		    IF @ReportStatusAttributeId IS NOT NULL
		    BEGIN
			    -- If found update the record status
			    UPDATE B SET 
				    [ResponseDate] = AV.[ModifiedDateTime],
				    [RecordFound] = CASE WHEN AV.[Value] = 'Review' THEN 1 ELSE 0 END
			    FROM [AttributeValue] AV
			    INNER JOIN [BackgroundCheck] B
				    ON B.[WorkflowId] = AV.[EntityId]
			    WHERE AV.[AttributeId] = @ReportStatusAttributeId
			    AND AV.[Value] <> ''
		    END

		    -- Get the attribute that stores the document
		    DECLARE @ReportAttributeId int = (
			    SELECT TOP 1 [Id] 
			    FROM [Attribute]
			    WHERE [EntityTypeId] = @WorkflowEntityTypeId
			    AND [EntityTypeQualifierValue] = CAST( @BackgroundCheckWorkflowTypeId as varchar )
			    AND [Key] = 'ResultAttribute' 
		    )
		    IF @ReportAttributeId IS NOT NULL
		    BEGIN
			    UPDATE B SET
				    [ResponseDocumentId] = F.[Id]
			    FROM [AttributeValue] AV
			    INNER JOIN [Attribute] A
				    ON A.[Guid] = AV.[Value]
			    INNER JOIN [BackgroundCheck] B
				    ON B.[WorkflowId] = AV.[EntityId]
			    INNER JOIN [PersonAlias] PA
				    ON PA.[Id] = B.[PersonAliasId]
			    INNER JOIN [AttributeValue] PAV
				    ON PAV.[AttributeId] = A.[Id]
				    AND PAV.[EntityId] = PA.[PersonId]
			    INNER JOIN [BinaryFile] F
				    ON F.[Guid] = PAV.[Value]
			    WHERE AV.[AttributeId] = @ReportAttributeId
			    AND PAV.[Value] <> ''
		    END

	    END

    END