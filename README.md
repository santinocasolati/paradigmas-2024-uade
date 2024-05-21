# Paradigmas de Programacion
## UADE 2024 - Santino Casolati

Proyecto dedicado a la materia de "Paradigmas de Programacion", cursada en UADE el primer cuatrimestre del año 2024 por el alumno Santino Casolati

## Casos de Composicion
Se aplica composicion en dos casos:

 - Characters y CharacterAnim: la clase CharacterAnim solo existe dentro del contexto de un Character especifico. En caso de que ese Character se elimine, las instancias de CharacterAnim tambien serian destruidas. Se utiliza de esta forma ya que no es necesario que la CharacterAnim exista si el Character no lo hace

![](https://github.com/santinocasolati/paradigmas-2024-uade/blob/master/Docs/composicion_1.jpg?raw=true)

 - LevelManager y Levels: similar al caso anterior, dentro de la clase LevelManager se produce composicion usando las clases que heredan de Level. Al ser creados dentro del LevelManager y gestionados por el mismo, no pueden existir por afuera de este. Se hace asi ya que, al ser el LevelManager el encargado de administrar los niveles, no es necesario que exista un nivel si este no esta para gestionarlo

![](https://github.com/santinocasolati/paradigmas-2024-uade/blob/master/Docs/composicion_2.jpg?raw=true)

## Casos de Agregacion
Se aplica composicion en dos casos:

 - GameUpdater y Player: el Player es creado en la clase GameplayLevel y es enviado como parametro al metodo SetPlayer del GameUpdater. Esto hace que el Player exista por fuera de este y en caso de ser eliminado el GameUpdater, el Player seguiria existiendo en la clase GameplayLevel
 
 - GameUpdater y Timer: sucede lo mismo en el caso de los Timers. Son creados en el GameplayLevel y enviados al GameUpdater como parametro de AddUpdatableObj para agregarlos en una lista

![](https://github.com/santinocasolati/paradigmas-2024-uade/blob/master/Docs/agregacion_1.jpg?raw=true)
![](https://github.com/santinocasolati/paradigmas-2024-uade/blob/master/Docs/agregacion_2.jpg?raw=true)

En ambos casos, se utiliza asi ya que si por algun motivo no esta el GameUpdater, la instancia del Character debe poder ser utilizada en otro GameUpdater nuevo

## Diagrama de Clases
Para realizar el diagrama de clases, utilice la herramienta plantUML

![](https://github.com/santinocasolati/paradigmas-2024-uade/blob/master/Docs/diagramaDeClases.png?raw=true)

```
@startuml
interface ICollidable {
    + Collide(otherType: CharacterType)
}

interface IDrawable {
    + Draw()
    + Update(deltaTime: float)
}

interface IRecievesInput {
    + Input()
}

interface IHasAnimations {
    + AddAnimations()
}

enum CharacterType {
    Red
    Green
    Blue
    Obstacle
}

enum LevelType {
    Menu
    Gameplay
    Win
    Lose
}

class Animation {
    - string id
    - bool isLoopEnabled
    - List<Texture> frames
    - float speed
    - float currentAnimationTime
    - int currentFrameIndex
    + Reset()
    + SetSpeed()
    + Update()
}

class GameManager {
    - float timeToWin
    - float timeToLose
    - float startingTime
    - GameTimer gameTimer
    + Reset()
    + AddTime(timeToAdd: float)
    + RemoveTime(timeToRemove: float)
    + UpdateTimer(deltaTime: float)
    + DrawTimer()
}

class GameTimer {
    - float currentTime
    - float maxTime
    - Animation clockAnim
    - Animation handAnim
    - float scale
    - float handRotation
    + AddTime(timeToAdd: float)
    + SetTime(currentTime: float, maxTime: float)
    + RemoveTime(timeToRemove: float)
    - InterpolateTime()
}

class GameUpdater {
    - Player player
    - List<Character> characterList
    + SetPlayer(player: Player)
    + AddUpdatableObj(character: Character)
    + RemoveUpdatableObj(character: Character)
    + Input()
    + Update(deltaTime: float)
    + Render()
    - CheckCollisions()
    - IsBoxColliding(posOne: Vector2, realSizeOne: Vector2, posTwo: Vector2, RealSizeTwo: Vector2): bool
}

class Character {
    # Vector2 size
    # Vector2 position
    # float rotation
    # CharacterType characterType
    # Animation currentAnimation
}

class Player {
    - float speed
    - float rotationSpeed
    - CharacterAnim redShip
    - CharacterAnim greenShip
    - CharacterAnim blueShip
    - ChangeColor()
    - SelectRandomAnimation() : CharacterAnim
    - CalculateUpVector(float rotation) : Vector2
    - AddSpeed(float deltaTime)
    - CheckBorders()
}

class Timer {
    - float lifeTime
    - int minLifeTime
    - int maxLifeTime
    - float currentTime
    - CharacterAnim redTimer
    - CharacterAnim greenTimer
    - CharacterAnim blueTimer
    - Generate()
    - SelectRandomAnimation() : CharacterAnim
}

class CharacterAnim {
    - CharacterType characterType
    - Animation animation
}

class Level {
    + Render()
}

class UpdatableLevel {
    + Update(deltaTime: float)
    + CreateLevel()
}

class GameplayLevel {
    - GameUpdater gameUpdater
    + Update(deltaTime: float)
}

class MenuLevel {
}

class WinLevel {
}

class GameOverLevel {
}

class LevelManager {
    - LevelManager instance
    - Level currentLevel
    + SetLevel(levelType: LevelType)
}

class Program {
    + float deltaTime
    - DateTime lastFrameTime
    + int WIDTH
    + int HEIGHT
    + Random random
    - Main(args: string[])
    - CalcDeltaTime()
    - Input()
    - Update()
    - Render()
}

class Vector2 {
    - float x
    - float y
    + ScaleVector(scale: float)
}

Character ..|> ICollidable
Character ..|> IDrawable
GameTimer ..|> IDrawable
Player ..|> IRecievesInput
Level ..|> IRecievesInput
Character ..|> IHasAnimations 
GameTimer ..|> IHasAnimations 

Character <|-- Player
Character <|-- Timer
Character <|-- CharacterAnim
Level <|-- UpdatableLevel
UpdatableLevel <|-- GameplayLevel
Level <|-- MenuLevel
Level <|-- WinLevel
Level <|-- GameOverLevel

Animation *-- Texture

GameManager *-- GameTimer
GameTimer *-- Animation

GameUpdater o-- Player
GameUpdater o-- "1..*" Character

Character *-- Vector2
Character *-- CharacterType
Character *-- Animation

Player *-- CharacterAnim
Timer *-- CharacterAnim

CharacterAnim *-- CharacterType
CharacterAnim *-- Animation

GameplayLevel *-- GameUpdater

LevelManager *-- Level
LevelManager *-- LevelType
@enduml
```
