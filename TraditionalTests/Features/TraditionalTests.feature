@Traditional

Feature: Traditional Tests

Scenario: Login Page UI Elements
   Given I open the login page
   Then I verify the expected fields exist in the page
   And I verify the expected field labels exist in the page
   And I verify the expected images exist in the page
   And I verify the expected link elements exist in the page
   And I verify the login button exists in the page
   And I verify the Remember Me checkbox exists in the page
   
Scenario Outline: User can log in with correct credentials
	Given I open the login page
   	When I enter username '<username>' and password '<password>'
	Then I verify the page contains text '<text>'
	Examples: 
		| username | password | text                                       |
		|          |          | Both Username and Password must be present |
		| user     |          | Password must be present                   |
		|          | pa$$w0rd | Username must be present                   |
		| user     | pa$$w0rd | Recent Transactions                        |

Scenario: The transactions table can be sorted by Amount 
	Given I open the login page
	And I enter username 'testUser' and password 'password'
	When I see a table of transactions
	And I click the amount column to sort it
	Then I verify that the table rows remained in tact
	And I verify the amount column is in ascending order

######################################################################################################################
# This test is better suited for a visual ui testing tool like Applitools because the page uses canvas.              #
# However, I thought it would be a challenge to try coding an image comparison utility against a baseline.           #
# I incorporated an image comparison algorithm I found on stack-overflow here:                                       #
# https://stackoverflow.com/questions/35151067/algorithm-to-compare-two-images-in-c-sharp                            #   
# This solution has limitations because baseline and v2 of the barchart didn't match but I got no insight as to why. #
# In a real life scenario, I would opt to use a visual ui testing tool instead. It's so much easier.                 #
######################################################################################################################
Scenario: Compare Expenses in the canvas chart
	Given I open the login page
	And I enter username 'testUser' and password 'password'
	When I click compare expenses and see a bar chart
	Then I validate the bar chart has the correct data
	And I validate next years data is displayed

Scenario: Dynamic Ads are displayed
	Given I open the login page with Ads enabled
	And I enter username 'testUser' and password 'password'
	Then I verify both flashSale ads are displayed on the page