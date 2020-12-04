# KeePass for Stream Deck 
## Description

Simple interface to retrieve information from a KeePass database and generate passwords using [KeePassLib](https://www.nuget.org/packages/KeePassLib/).

I wanted to learn how to use the [Elgato Stream Deck SDK](https://developer.elgato.com/documentation/stream-deck/sdk/overview/) using BarRaider's [Stream Deck Tools](https://github.com/BarRaider/streamdeck-tools), so I created this simple plug-in.

## Features

#### Retrieve

It retrieves a field of a stored entry from a `.kdbx` file and copies it to the clipboard.

It uses the following configuration:

- **KeePass db path.** The absolute path to the KeePass file.
- **KeePass db password.** The main password of the KeePass file.
- **Entry Name.** The name of the entry that has the desired information.
- **Field to retrieve.** The field whose content will be copied to the clipboard
  - Password
  - Username
  - Notes
  - URL



#### Generate

it will generate a random password using [KeePassLib](https://www.nuget.org/packages/KeePassLib/) using the given configuration.

It uses the following configuration:

- **Password Length.** Default `20`.
- **Lower Case.** `abcdefghijklmnopqrstuvwxyz`. Default `true`.
- **Upper Case.** `ABCDEFGHIJKLMNOPQRSTUVWXYZ`. Default `true`.
- **Use Digits.** `0123456789`. Default `true`.
- **Use Punctuation.** `,.;:`. Default `true`.
- **Use Brackets.** `()[]{}<>`. Default `true`.
- **Use Special Characters.** ``!"#$%&'()*+,-./:;<=>?@[\]^_`{|}~``. Default `true`.
- **Exclude Look-Alike characters.** It will avoid using two character that look similar, like `O` and `0`. Default `false`.
- **Must occur at most Once.** It will prevent characters from appearing more than once. Default `false`.
- **Characters to Exclude.** Any character included in the text field will be excluded from the generated password.
- **Custom Pattern.** Allows to use a custom defined password generation pattern. Refer to the section *Generating Passwords that Follow Rules* of [KeePass Password Generator](https://keepass.info/help/base/pwgenerator.html) documentation.
  - ***Note***: Using a custom pattern will override all the previous configuration.

Check [KeePass Password Generator](https://keepass.info/help/base/pwgenerator.html) help site for more information.

## My other Stream Deck plugins

- **[Color Picker](https://github.com/VictorGrycuk/streamdeck-color-picker)**
- **[Magnifier](https://github.com/VictorGrycuk/streamdeck-magnifier)**
- **[Repository Watcher](https://github.com/VictorGrycuk/streamdeck-repository-watcher)**

---

The icon for this action is a modified version of *iOS Filled* at [Icon8](https://icons8.com).