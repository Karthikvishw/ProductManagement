

USE [ProductManagement]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ProductStatus](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_ProductStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ProductCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[AdditionalNote] [varchar](50) NOT NULL,
	[ProductCategoryId] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
	[Deleted] [bit] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_ProductCategory] FOREIGN KEY([ProductCategoryId])
REFERENCES [dbo].[ProductCategory] ([Id])
GO

ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_ProductCategory]
GO

ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_ProductStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[ProductStatus] ([Id])
GO

ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_ProductStatus]
GO


INSERT INTO [dbo].[ProductCategory]([Name],[Deleted]) VALUES ('Automotive' ,0)
GO
INSERT INTO [dbo].[ProductCategory]([Name],[Deleted]) VALUES ('Clothing' ,0)
GO
INSERT INTO [dbo].[ProductCategory]([Name],[Deleted]) VALUES ('Furniture' ,0)
GO
INSERT INTO [dbo].[ProductCategory]([Name],[Deleted]) VALUES ('Home' ,0)
GO


INSERT INTO [dbo].[ProductStatus] ([Name] ,[Deleted]) VALUES ('In stock' ,0)
GO
INSERT INTO [dbo].[ProductStatus] ([Name] ,[Deleted]) VALUES ('Out of stock' ,0)
GO
INSERT INTO [dbo].[ProductStatus] ([Name] ,[Deleted]) VALUES ('Order Placed' ,0)
GO