INSERT INTO SystemAccount (AccountName, AccountEmail, AccountRole, AccountPassword)
VALUES
(N'Nguyen Van An',  'an.nguyen@example.com',   'Editor',   'hashed_pw_1'),
(N'Tran Thi Binh',  'binh.tran@example.com',   'Reporter', 'hashed_pw_2'),
(N'Le Minh Chau',   'chau.le@example.com',     'Reporter', 'hashed_pw_3'),
(N'Pham Quoc Dat',  'dat.pham@example.com',    'Editor',   'hashed_pw_4'),
(N'Hoang Anh Duc',  'duc.hoang@example.com',   'Reporter', 'hashed_pw_5');

INSERT INTO Category (CategoryName, CategoryDescription, ParentCategoryID, IsActive)
VALUES
(N'Technology', N'Tech related news', NULL, 1),
(N'Education',  N'Education and learning', NULL, 1),
(N'Software',   N'Software development', 1, 1),
(N'AI',         N'Artificial Intelligence', 1, 1),
(N'University', N'University news', 2, 1);

INSERT INTO NewsArticle
(NewsTitle, Headline, NewsContent, NewsSource, CategoryID, NewsStatusID, CreatedByID, UpdatedByID, ModifiedDate)
VALUES
(
 N'AI Transforming Software Industry',
 N'How AI changes modern development',
 N'AI is rapidly changing how software is designed, built, and tested.',
 N'Tech Daily',
 4, 3, 1, NULL, NULL
),
(
 N'University Curriculum Update 2026',
 N'New subjects for IT students',
 N'Many universities are updating their curriculum to include AI and cloud computing.',
 N'Education Times',
 5, 3, 2, 3, SYSUTCDATETIME()
),
(
 N'Clean Architecture in .NET',
 N'Best practices for scalable systems',
 N'Clean Architecture helps developers build maintainable and testable applications.',
 NULL,
 3, 2, 4, NULL, NULL
),
(
 N'Students Embrace GitHub',
 N'GitHub becomes core learning tool',
 N'More students are using GitHub daily for coursework and collaboration.',
 N'Campus News',
 5, 3, 5, 2, SYSUTCDATETIME()
),
(
 N'Future of Software Engineering',
 N'Trends to watch',
 N'AI, low-code, and cloud-native systems dominate the future.',
 N'Software Weekly',
 3, 1, 1, NULL, NULL
);

INSERT INTO Tag (TagName, Note)
VALUES
(N'AI',        N'Artificial Intelligence related'),
(N'.NET',      N'.NET ecosystem'),
(N'Education', N'Education sector'),
(N'GitHub',    N'GitHub usage'),
(N'Software',  N'General software topics');

INSERT INTO NewsTag (NewsArticleID, TagID)
VALUES
(1, 1),  -- AI Transforming Software Industry → AI
(2, 3),  -- University Curriculum Update → Education
(3, 2),  -- Clean Architecture in .NET → .NET
(4, 4),  -- Students Embrace GitHub → GitHub
(5, 5);  -- Future of Software Engineering → Software
