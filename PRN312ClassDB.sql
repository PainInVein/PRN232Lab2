USE PRN312ClassDB
GO


-- 1. SystemAccount (base table for users who manage news)
CREATE TABLE SystemAccount (
    AccountID       INT IDENTITY(1,1) PRIMARY KEY,
    AccountName     NVARCHAR(100) NOT NULL,
    AccountEmail    NVARCHAR(150) NOT NULL UNIQUE,
    AccountRole     NVARCHAR(50) NOT NULL,           -- e.g. 'Admin', 'Editor', 'Reporter'
    AccountPassword NVARCHAR(255) NOT NULL            -- should be hashed
);
GO
-- Suggestion: add these useful columns later if needed
-- CreatedDate     DATETIME2 DEFAULT SYSUTCDATETIME(),
-- LastLogin       DATETIME2 NULL


-- 2. Category (hierarchical - supports parent/child categories)
CREATE TABLE Category (
    CategoryID          INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName        NVARCHAR(150) NOT NULL,
    CategoryDescription NVARCHAR(500) NULL,
    ParentCategoryID    INT NULL,                     -- self-reference for hierarchy
    IsActive            BIT NOT NULL DEFAULT 1,

    CONSTRAINT FK_Category_Parent 
        FOREIGN KEY (ParentCategoryID) 
        REFERENCES Category(CategoryID)
);
GO
-- Tip: consider adding a unique index on (ParentCategoryID, CategoryName) 
-- if you want to prevent duplicate names in the same parent


-- 3. NewsArticle (main news content)
CREATE TABLE NewsArticle (
    NewsArticleID   INT IDENTITY(1,1) PRIMARY KEY,
    NewsTitle       NVARCHAR(500) NOT NULL,
    Headline        NVARCHAR(500) NULL,               -- subtitle / lead
    CreatedDate     DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    NewsContent     NVARCHAR(MAX) NOT NULL,           -- full article body
    NewsSource      NVARCHAR(200) NULL,               -- original source if reposted
    CategoryID      INT NOT NULL,
    NewsStatusID    INT NOT NULL,                     -- you may want to replace with ENUM/check later
    CreatedByID     INT NOT NULL,
    UpdatedByID     INT NULL,
    ModifiedDate    DATETIME2 NULL,

    CONSTRAINT FK_NewsArticle_Category 
        FOREIGN KEY (CategoryID) 
        REFERENCES Category(CategoryID),

    CONSTRAINT FK_NewsArticle_CreatedBy 
        FOREIGN KEY (CreatedByID) 
        REFERENCES SystemAccount(AccountID),

    CONSTRAINT FK_NewsArticle_UpdatedBy 
        FOREIGN KEY (UpdatedByID) 
        REFERENCES SystemAccount(AccountID)
);
GO
-- Optional: better status handling (instead of just NewsStatusID as int)
-- You can either:
-- A. Create a separate NewsStatus table
-- B. Use CHECK constraint
-- C. Use a computed/display column later

-- Example with CHECK (simple version):
ALTER TABLE NewsArticle
ADD CONSTRAINT CHK_NewsStatusID 
CHECK (NewsStatusID IN (1,2,3,4,5));  -- e.g. 1=draft, 2=review, 3=published, 4=archived, 5=deleted
GO

-- 4. Tag (simple tag master table)
CREATE TABLE Tag (
    TagID     INT IDENTITY(1,1) PRIMARY KEY,
    TagName   NVARCHAR(100) NOT NULL UNIQUE,
    Note      NVARCHAR(500) NULL
);
GO

-- 5. NewsTag (junction table - many-to-many between NewsArticle & Tag)
CREATE TABLE NewsTag (
    NewsArticleID   INT NOT NULL,
    TagID           INT NOT NULL,

    CONSTRAINT PK_NewsTag 
        PRIMARY KEY (NewsArticleID, TagID),

    CONSTRAINT FK_NewsTag_NewsArticle 
        FOREIGN KEY (NewsArticleID) 
        REFERENCES NewsArticle(NewsArticleID)
        ON DELETE CASCADE,

    CONSTRAINT FK_NewsTag_Tag 
        FOREIGN KEY (TagID) 
        REFERENCES Tag(TagID)
        ON DELETE CASCADE
);
GO