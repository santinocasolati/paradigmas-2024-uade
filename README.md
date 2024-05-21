# Paradigmas de Programacion
## UADE 2024 - Santino Casolati

Proyecto dedicado a la materia de "Paradigmas de Programacion", cursada en UADE el primer cuatrimestre del año 2024 por el alumno Santino Casolati

## Casos de Composicion
Se aplica composicion en dos casos:

 - Characters y CharacterAnim: la clase CharacterAnim solo existe dentro del contexto de un Character especifico. En caso de que ese Character se elimine, las instancias de CharacterAnim tambien serian destruidas. Se utiliza de esta forma ya que no es necesario que la CharacterAnim exista si el Character no lo hace

 - LevelManager y Levels: similar al caso anterior, dentro de la clase LevelManager se produce composicion usando las clases que heredan de Level. Al ser creados dentro del LevelManager y gestionados por el mismo, no pueden existir por afuera de este. Se hace asi ya que, al ser el LevelManager el encargado de administrar los niveles, no es necesario que exista un nivel si este no esta para gestionarlo

## Casos de Agregacion
Se aplica composicion en dos casos:

 - GameUpdater y Player: el Player es creado en la clase GameplayLevel y es enviado como parametro al metodo SetPlayer del GameUpdater. Esto hace que el Player exista por fuera de este y en caso de ser eliminado el GameUpdater, el Player seguiria existiendo en la clase GameplayLevel
 
 - GameUpdater y Timer: sucede lo mismo en el caso de los Timers. Son creados en el GameplayLevel y enviados al GameUpdater como parametro de AddUpdatableObj para agregarlos en una lista

En ambos casos, se utiliza asi ya que si por algun motivo no esta el GameUpdater, la instancia del Character debe poder ser utilizada en otro GameUpdater nuevo
