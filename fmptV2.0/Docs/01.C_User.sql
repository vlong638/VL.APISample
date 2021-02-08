IF OBJECT_ID(N'User', N'U') IS  NOT  NULL 
DROP TABLE [User];

SET QUOTED_IDENTIFIER ON
GO

-- 新增时间 设置默认值
CREATE TABLE [dbo].[User](
	[Id][bigint] IDENTITY(1, 2) NOT NULL,
	-------------------------------------------上关联
	[db_createtime] [datetime] DEFAULT CURRENT_TIMESTAMP,
	[db_updatetime] [datetime] NULL,
	-------------------------------------------下管控
	IsDeleted bit default 0,
	-------------------------------------------下内容
	[Name] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
	[Password] nvarchar(32) COLLATE Chinese_PRC_CI_AS  NULL,
	[NickName] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
	[Phone] nvarchar(20) COLLATE Chinese_PRC_CI_AS  NULL,
	[Sex] tinyint  NULL,
	[CreatedAt] datetime  NULL
	-------------------------------------------
	CONSTRAINT[PK_User] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON) ON[PRIMARY]
) ON[PRIMARY]
GO

-- 更新时间 采用触发器
CREATE TRIGGER [dbo].[trigger_User_db_updatetime]
ON[dbo].[User]
FOR UPDATE
AS
BEGIN
  update [User] set db_updatetime = CURRENT_TIMESTAMP
	where id in (select id from inserted)
END
GO

-- 校验
select * from [User];


IF ((SELECT COUNT(*) FROM ::fn_listextendedproperty('MS_Description',
'SCHEMA', N'dbo',
'TABLE', N'User',
'COLUMN', N'Name')) > 0)
  EXEC sp_updateextendedproperty
'MS_Description', N'姓名',
'SCHEMA', N'dbo',
'TABLE', N'User',
'COLUMN', N'Name'
ELSE
  EXEC sp_addextendedproperty
'MS_Description', N'姓名',
'SCHEMA', N'dbo',
'TABLE', N'User',
'COLUMN', N'Name'
GO

IF ((SELECT COUNT(*) FROM ::fn_listextendedproperty('MS_Description',
'SCHEMA', N'dbo',
'TABLE', N'User',
'COLUMN', N'Password')) > 0)
  EXEC sp_updateextendedproperty
'MS_Description', N'密码',
'SCHEMA', N'dbo',
'TABLE', N'User',
'COLUMN', N'Password'
ELSE
  EXEC sp_addextendedproperty
'MS_Description', N'密码',
'SCHEMA', N'dbo',
'TABLE', N'User',
'COLUMN', N'Password'
GO

IF ((SELECT COUNT(*) FROM ::fn_listextendedproperty('MS_Description',
'SCHEMA', N'dbo',
'TABLE', N'User',
'COLUMN', N'NickName')) > 0)
  EXEC sp_updateextendedproperty
'MS_Description', N'昵称',
'SCHEMA', N'dbo',
'TABLE', N'User',
'COLUMN', N'NickName'
ELSE
  EXEC sp_addextendedproperty
'MS_Description', N'昵称',
'SCHEMA', N'dbo',
'TABLE', N'User',
'COLUMN', N'NickName'
GO

IF ((SELECT COUNT(*) FROM ::fn_listextendedproperty('MS_Description',
'SCHEMA', N'dbo',
'TABLE', N'User',
'COLUMN', N'Phone')) > 0)
  EXEC sp_updateextendedproperty
'MS_Description', N'手机号',
'SCHEMA', N'dbo',
'TABLE', N'User',
'COLUMN', N'Phone'
ELSE
  EXEC sp_addextendedproperty
'MS_Description', N'手机号',
'SCHEMA', N'dbo',
'TABLE', N'User',
'COLUMN', N'Phone'
GO

IF ((SELECT COUNT(*) FROM ::fn_listextendedproperty('MS_Description',
'SCHEMA', N'dbo',
'TABLE', N'User',
'COLUMN', N'Sex')) > 0)
  EXEC sp_updateextendedproperty
'MS_Description', N'性别',
'SCHEMA', N'dbo',
'TABLE', N'User',
'COLUMN', N'Sex'
ELSE
  EXEC sp_addextendedproperty
'MS_Description', N'性别',
'SCHEMA', N'dbo',
'TABLE', N'User',
'COLUMN', N'Sex'
GO

IF ((SELECT COUNT(*) FROM ::fn_listextendedproperty('MS_Description',
'SCHEMA', N'dbo',
'TABLE', N'User',
'COLUMN', N'CreatedAt')) > 0)
  EXEC sp_updateextendedproperty
'MS_Description', N'创建时间',
'SCHEMA', N'dbo',
'TABLE', N'User',
'COLUMN', N'CreatedAt'
ELSE
  EXEC sp_addextendedproperty
'MS_Description', N'创建时间',
'SCHEMA', N'dbo',
'TABLE', N'User',
'COLUMN', N'CreatedAt'


CREATE NONCLUSTERED INDEX [idx_User_Id]
ON [dbo].[User] (
  [Id]
)
GO

CREATE NONCLUSTERED INDEX [idx_User_IsDeleted]
ON [dbo].[User] (
  [IsDeleted]
)