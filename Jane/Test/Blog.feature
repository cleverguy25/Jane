Feature: Blog List

@IntegrationTests
Scenario: Blog List with Schema
	When I navigate to 'http://localhost:10973/'
	Then I should see the following posts
   | Title              | Author            | PubDate     |
   | My First Blog Post | cleve.littlefield | 2014 May 12 |
   | Ha! Another one.   | cleve.littlefield | 2014 May 13 |
   | Ha! Three.         | cleve.littlefield | 2014 May 14 |

@IntegrationTests
Scenario: Single Post with Schema
	When I navigate to 'http://localhost:10973/blog/first-post'
	Then I should see the following posts
   | Title              | Author            | PubDate     |
   | My First Blog Post | cleve.littlefield | 2014 May 12 |

@IntegrationTests
Scenario: Blog Tags
	When I navigate to 'http://localhost:10973/blog/tagged/foo'
	Then I should see the following posts
   | Title              | Author            | PubDate     |
   | My First Blog Post | cleve.littlefield | 2014 May 12 |
   | Ha! Another one.   | cleve.littlefield | 2014 May 13 |