Feature: Blog List

Scenario: Blogs
	When I navigate to 'http://localhost:10973/'
	Then should have the following posts
   | Title              | Author            | PubDate     |
   | My First Blog Post | cleve.littlefield | 2014 May 12 |
   | Ha! Another one.   | cleve.littlefield | 2014 May 13 |
   | Ha! Three.         | cleve.littlefield | 2014 May 14 |