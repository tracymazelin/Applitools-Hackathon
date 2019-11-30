# Applitools Hackathon 2019

The challenge is to write five (5) automated tests for both versions of the Applitools demo app:

1. One suite using your preferred traditional functional testing approach
2. Another suite which covers the same tests but uses visual AI testing with Applitools

More details about the competion requirements can be found [here](https://applitools.com/main-concepts).

## Overview

I chose to complete part one using the SpecFlow BDD testing framework.  I chose it for a few reasons:

1. It meets the project requirements for using selenium webdriver with c# bindings
2. I use this framework in my job daily and so I'm familiar with it
3. BDD testing is an industry best practice and implements the Page Object Model

## Pacakages

The project uses the following packages:

* Selenium Web Driver v3.141
* ChromeDriver v78.0.3904.10500 (requires Chrome browser v78)
* SpecFlow v3.1.62 (open source BDD testing framework)
* NUnit v3.11.0
* NUnit3TestAdapter v3.15.1 (you won't need a specrunner license to execute these tests)


## Project Structure

* SpecFlow tests are organized as Features, Pages, and Steps.  
* The features folder contains a feature file (TraditionalTests.feature) where all the tests are written with human readable acceptance criteria.
* The scenarios are bound to a steps class in the Steps folder (TraditionalTestSteps.cs).  This is the heart of the manual test code.
* I added an image capture utility for comparing images.  Notes about this can be found in the feature file.
* There is a WebDriver hook that gets setup and torn down after each test.  It also sets a selenium implicit wait along with some chrome options.
* The VisualAI tests are in the VisualAITests class and utilize Applitools Eyes SDK  
	* It is amazing that this one class does all the same work and finds all the same bugs (and more) as the traditional test approach!
	* The visual bugs are all documented in the Applitools Test Manager
* The version the tests execute against can be changed via variables utilizing the configuration manager:
	* ConfigurationManager.AppSettings["V1"]
	* ConfigurationManager.AppSettings["V2"]

## Pre-requisites

* Windows OS
* Visual Studio
* Chrome 78

## Installation

* Clone the repo
* Add your applitools api key to an environment variable named 'APPLITOOLS_API_KEY'
* Build the solution
* Execute the tests using the Visual Studio Test Explorer

## Author
* Tracy Mazelin