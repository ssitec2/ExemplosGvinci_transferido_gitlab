/*------------------------------------------------------------*/
/*   Esquema para a criação do banco de dados da aplicação    */
/*                         22326MAKOTO                        */
/*------------------------------------------------------------*/

/*------------------------------------------------------------*/
/*                     Exclusão de Triggers                   */
/*------------------------------------------------------------*/

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('TB_LOGIN_GROUP_RULE') AND sysstat & 0xf = 11)
ALTER TABLE [TB_LOGIN_RULE]
DROP CONSTRAINT [TB_LOGIN_GROUP_RULE]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('TB_LOGIN_GROUP_USER') AND sysstat & 0xf = 11)
ALTER TABLE [TB_LOGIN_USER]
DROP CONSTRAINT [TB_LOGIN_GROUP_USER]
GO

/*------------------------------------------------------------*/
/*                     Exclusão de Views                      */
/*------------------------------------------------------------*/

/*------------------------------------------------------------*/
/*                     Exclusão de tabelas                    */
/*------------------------------------------------------------*/

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('TB_LOGIN_GROUP') AND sysstat & 0xf = 3)
DROP TABLE [TB_LOGIN_GROUP]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('TB_LOGIN_RULE') AND sysstat & 0xf = 3)
DROP TABLE [TB_LOGIN_RULE]
GO

IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('TB_LOGIN_USER') AND sysstat & 0xf = 3)
DROP TABLE [TB_LOGIN_USER]
GO

/*------------------------------------------------------------*/
/*                     Criação das tabelas                    */
/*------------------------------------------------------------*/

/*------------------------------------------------------------*/
/*    Criação de Tabelas, Indices e Atribuição de Default    */
/*       TB_LOGIN_GROUP       */
/*------------------------------------------------------------*/

 CREATE TABLE [TB_LOGIN_GROUP](
	[LOGIN_GROUP_NAME]                     varchar (60)         NOT NULL,
	[LOGIN_GROUP_IS_ADMIN]                 bit                  NOT NULL
		CONSTRAINT [LOGIN_GROUP_PK] PRIMARY KEY CLUSTERED
		(
			[LOGIN_GROUP_NAME]
		) WITH FILLFACTOR = 90
)
GO

/*------------------------------------------------------------*/
/*    Criação de Tabelas, Indices e Atribuição de Default    */
/*      TB_LOGIN_RULE      */
/*------------------------------------------------------------*/

 CREATE TABLE [TB_LOGIN_RULE](
	[LOGIN_RULE_PROJECT]                   varchar (8)          NOT NULL,
	[LOGIN_GROUP_NAME]                     varchar (60)         NOT NULL,
	[LOGIN_RULE_OBJECT]                    varchar (100)        NOT NULL,
	[LOGIN_RULE_PERMISSIONS]               varchar (100)        NOT NULL
		CONSTRAINT [LOGIN_RULE_PK] PRIMARY KEY CLUSTERED
		(
			[LOGIN_RULE_PROJECT],[LOGIN_GROUP_NAME],[LOGIN_RULE_OBJECT]
		) WITH FILLFACTOR = 90
)
GO

/*------------------------------------------------------------*/
/*    Criação de Tabelas, Indices e Atribuição de Default    */
/*      TB_LOGIN_USER      */
/*------------------------------------------------------------*/

 CREATE TABLE [TB_LOGIN_USER](
	[LOGIN_GROUP_NAME]                     varchar (60)         NOT NULL,
	[LOGIN_USER_LOGIN]                     varchar (40)         NOT NULL,
	[LOGIN_USER_PASSWORD]                  varchar (40)         NOT NULL,
	[LOGIN_USER_NAME]                      varchar (60)         NOT NULL,
	[LOGIN_USER_OBS]                       text                 NOT NULL
		CONSTRAINT [LOGIN_USER_PK] PRIMARY KEY CLUSTERED
		(
			[LOGIN_USER_LOGIN]
		) WITH FILLFACTOR = 90
)
GO

ALTER TABLE [TB_LOGIN_RULE] ADD CONSTRAINT [TB_LOGIN_GROUP_RULE]
	FOREIGN KEY
		([LOGIN_GROUP_NAME])
	REFERENCES [TB_LOGIN_GROUP]
		([LOGIN_GROUP_NAME])
	ON DELETE CASCADE
GO

ALTER TABLE [TB_LOGIN_USER] ADD CONSTRAINT [TB_LOGIN_GROUP_USER]
	FOREIGN KEY
		([LOGIN_GROUP_NAME])
	REFERENCES [TB_LOGIN_GROUP]
		([LOGIN_GROUP_NAME])
	ON DELETE CASCADE
GO

