# Console Flash Cards
A flash card console application is designed to help users study by adding information onto a stack of cards.
Cards will be drawn randomly from stack and user will be prompted with a question to answer.

Developed using C# and MSQL.

# Requirements
- [x] This is an application where the users will create Stacks of Flashcards.
- [x] You'll need two different tables for stacks and flashcards. The tables should be linked by a foreign key.
- [x] Stacks should have an unique name.
- [x] Every flashcard needs to be part of a stack. If a stack is deleted, the same should happen with the flashcard.
- [x] You should use DTOs to show the flashcards to the user without the Id of the stack it belongs to.
- [x] When showing a stack to the user, the flashcard Ids should always start with 1 without gaps between them. If you have 10 cards and number 5 is deleted, the table should show Ids from 1 to 9.
- [ ] After creating the flashcards functionalities, create a "Study Session" area, where the users will study the stacks. All study sessions should be stored, with date and score.
- [ ] The study and stack tables should be linked. If a stack is deleted, it's study sessions should be deleted.
- [ ] The project should contain a call to the study table so the users can see all their study sessions. This table receives insert calls upon each study session, but there shouldn't be update and delete calls to it.


# Challenges
- [ ] Create a report system where you can see the number of sessions per month per stack
- [ ] Average score per month per stack.

## Features

## Installation

## Resources
- [Spectre Console documentation](https://spectreconsole.net/)
- [Using Configuration Manager](https://docs.microsoft.com/en-us/troubleshoot/dotnet/csharp/store-custom-information-config-file)
- StackOverflow Articles