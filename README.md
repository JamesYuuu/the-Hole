# The Hole

![hole above](./Docs/holeAbove.png)
![hole under](./Docs/holeUnder.png)
A virtual reality diving game made as part of CS3247 coursework. Tested on Oculus Rift, (Meta) Quest and Quest 2.

## Description

![island](./Docs/island.png)
Years ago, you lost your arm on a beach day when the fish in your village's lagoon turned hostile. Now, equipped with your new prosthetic grappling arm, it's time to get to the bottom of it. Literally. Swim, grapple, and blast your way through the deep in this virtual reality role-playing diving game. And don't forget to tease the wits out of your shopkeeper along the way.

## Notable Features

![hands](./Docs/shopHands.png)
- 'Natural user interface' user interaction paradigm. Inventory, shop interactions, status indicators (radar, oxygen) all aiming to mimic real-life interactions in physical space
- Custom mechanical hand model with animations, coupled with grappling movement system made in-house
- Pains taken to reduce motion sickness with vignette
- Custom water and water vision shaders written in-house
- Custom enemy AI that pathfinds towards player
- Characterisation of Nina and plot immerses the player, with random dialogues in the shop such that conversation does not get stale

## Developers

| Developer           | Contribution                                                                                                             |
|---------------------|--------------------------------------------------------------------------------------------------------------------------|
| Ni Yilun            | World model and prop models, water shader, water vision shader, scene transition, treasure spawning                      |
| Ng Jun Wei, Timothy | Grappling, player movement and controls, shooting system, hand model and animation                                       |
| Xu Yukun            | General modelling and animation, enemy spawning/AI, player inventory system, general integration, housekeeping           |
| Wang Huayu (Gary)   | Inventory system, user interface systems (radar, oxygen, grabbable items)                                                |
| Yu Junda (James)   | Shop and price systems, item system, Integration for Huayu                                                               |
| Hong Yi En, Ian     | User experience, project management, dialogue system, tutorial system and icons, story and characterisation, integration |

Pair programming was conducted extensively.

## Installation System Requirements

- Unity 2021.3.13f1 with Android Build Support

Packages

- 2D SpriteShape 7.0.6
- ARCore XR Plugin 4.2.7
- JetBrains Rider Editor 3.0.15
- Oculus XR Plugin 3.2.2
- OpenXR Plugin 1.5.3
- Post Processing 3.2.2
- Probuilder 5.0.6
- Test Framework 1.1.31
- TextMeshPro 3.0.6
- Timeline 1.6.4
- Toolchain Win Linux x64 2.0.4
- Tutorial Frameowkr 2.2.2
- Unity UI 1.0.0
- Universal RP 12.1.7
- Version Control 1.17.6
- Visual Scripting 1.7.8
- Visual Studio Code Editor 1.2.5
- Visual Studio Editor 2.0.16
- XR Interaction Toolkit 2.3.0
- XR Plugin Management 4.2.1

## Getting started

1. Clone this project and open it in Unity, ensuring that you have the installation requirements fulfilled

## Project status, Contributing

Development for this project has stopped completely after the completion of the semester.
If you wish to fork the project, do leave an issue.

### Commit message format

```csharp
// better commit messages

feat(lang): add polish language
feat(api)!: send an email to customer after product shipped
fix(import): converts file names to lowercase
chore(*): drop support for Node 6
docs(*): correct spelling of CHANGELOG
```

feat: a new feature

fix: a bug fix

style: changes that don’t affect the meaning of the code (white space, formatting, semicolons)

chore: other changes that don’t modify src or test files

docs: documentation only changes

refactor: a code change that neither fixes a bug nor adds a feature

perf: a code change that improves performance.

test: add missing tests or correcting existing tests

build: changes that affect the build system or external dependencies

ci: changes to our CI config files and scripts (Travis, Circle)

revert: reverts a previous commit

- add ! after the type to indicate that it is a breaking change

- add the scope in brackets to indicate the scope. (eg. the component or file name). if it affects everything, it should be (\*)

### Roadmap

- None.

## Acknowledgment

Made In-house

- fish models and animation by Xu Yukun
- robotic right hand model by Timothy Ng
- seagrass, coral models by Ni Yilun
- oculus controller icons and secondary dialog bubble by Ian Hong

### Models

Sketchfab

- [shop model by FabioNuzzo90](https://sketchfab.com/3d-models/fantasy-handpainted-shop-131d82bda0bc446a84b4a0eebd389ce2)
- [cash register model by bretzel44](https://sketchfab.com/3d-models/cash-register-1efcbed7fd124a89a9265c693d91b655)
- [energy drink by dark-minaz](https://sketchfab.com/3d-models/monster-energy-drink-d9e8651b357e4b11909ae305024e1d37)
- [diving helmet by Nadir Alishly](https://sketchfab.com/3d-models/diving-helmet-5073ee26106a467492e4943905c57ce2)
- [diving mask by pixelated_stars](https://sketchfab.com/3d-models/scuba-mask-a12059a9ff8f45e1979bd1af6fc1b8b2)
- [grappling hook by Aegis_Wolf](https://sketchfab.com/3d-models/halo-banished-brutehook-shotgun-63bc60bdc55d443c986f82fd9b419f65)
- [diving equipment by Steren](https://sketchfab.com/3d-models/scuba-equipment-25491e0d3a7d4c2c8ebb575307d830f7)
- [diving fins by FrancescoMilanese](https://sketchfab.com/3d-models/swimming-fins-1-cb7f4c712682470687a59233b466e369)
- [oxygen tank by pixelated_stars](https://sketchfab.com/3d-models/diving-tank-6d6c589bae0f4fb893a2bbb6cf895698)

- [door by NoobiePie](https://sketchfab.com/3d-models/low-poly-door-ae823f24039a4ecdbad86fe545778d4d)
- [table by Berk Gedik](https://sketchfab.com/3d-models/simple-table-low-poly-847fa35fec684e9ca57f4e3f8bdc25d0)

- jetty by
- village models by yilun
- rock models by yilun
- car models by yilun
- road models by yilun
- seaweed models by yilun
- coral reef models by yilun


Kenney

- gun and bullets from 

Yukun

- fishes
- shooting platform
- treasure goblet

### Textures, Sprites

Textures

- grass, rock textures by yilun
- CasualDay skybox by someone on unity asset store

Sprites

Kenney
- oxygen bar and dialogue background panel from [UI Pack (RPG Expansion)](https://kenney.nl/assets/ui-pack-rpg-expansion)

Font

- [fibberfish by nathan scott on itch.io](https://caffinate.itch.io/fibberish)

### Sounds

Freesound
- [light switch sound by tbrook](https://freesound.org/people/tbrook/sounds/348223/)
- [cash register sound by kiddpark](https://freesound.org/people/kiddpark/sounds/201159/)
- [fish dying splat sound by nebulasnails](https://freesound.org/people/nebulasnails/sounds/495117/)

Youtube
- water splash sound by
- water bubble sound by 
- gun sounds by

### Contributors
- Jedidiah Cheng (@jedidiahC) for the Hitchhiker's Guide to the Galaxy reference in one of the shop farewell messages.

## License
This project is not meant to be open sourced, and all code belongs to the respective authors in this group.
