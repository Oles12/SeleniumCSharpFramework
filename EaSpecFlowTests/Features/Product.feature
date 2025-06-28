Feature: Product
    Create a new product

Link to a feature: [Calculator]($projectname$/Features/Calculator.feature)
***Further read***: **[Learn more about how to generate Living Documentation](https://docs.specflow.org/projects/specflow-livingdoc/en/latest/LivingDocGenerator/Generating-Documentation.html)**


 Scenario:  Create product and verify the details
	Given I click the product menu
	And I click the "Create" link
	And I create product with following details
	    | Name      | Description        |Price   |ProductType|
	    | Headphones| Noise cancellation |300     |PERIPHARALS|
	When I click the Details link of the newly created product    
	Then I see all the product details are created as expected
	
	
