USE [image]
GO
/****** Object:  Table [dbo].[media]    Script Date: 4/26/2025 2:26:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[media](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[model] [nvarchar](100) NULL,
	[collection_name] [nvarchar](255) NULL,
	[name] [nvarchar](255) NULL,
	[id_related] [int] NULL,
	[file_size] [nvarchar](255) NULL,
	[CreatedAt] [datetime2](7) NULL,
	[UpdatedAt] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
