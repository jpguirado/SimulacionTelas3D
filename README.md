# SimulacionTelas3D

Práctica para la asignatura de animacion 3D. En esta práctica se implementa un componentes MassSpringCloth que añadido a un plano 2D permite simular el mismo como si fuese una tela. Para ello crea una malla virtual con cada uno de los vertices de la malla poligonal del plano y aplicando fórmulas fisicas se consigue el resultado de la simulación.

## Video demostración del funcionamiento de la práctica
[![Simulacion de telas 3D](https://img.youtube.com/vi/zsRLAmJmw5s/0.jpg)](https://www.youtube.com/watch?v=zsRLAmJmw5s "Simulación de telas 3D")

## Instrucciones de ejecución
Para abrir el proyecto es necesaria la versión 2017.3.1f1. Una vez abierto el proyecto, cargáis la escena haciendo doble clic en Exercise 1 si no se ha abierto sola. Pulsáis el botón de play y presionáis la tecla p para iniciar la simulación

## Ejecución de la simulación y elección de parámetros

Para la realización de la simulación simplemente hay que añadir el componente
MassSpringCloth a un GameObject que contenga una malla individual, como puede ser el objeto
Plane de Unity. Una vez añadido este componente al objeto podremos manejar los parámetros
de la simulación desde el mismo. Los parámetros que se pueden manejar son: rigidez de los
muelles de tracción (Stiffness), rigidez de los muelles de flexión (Stiffness flexion), masa de los
nodos (Mass), gravedad y método de integración. Otros parámetros configurables en la
simulación son los de DampingNode y DampingSpring, que regulan el fenómeno del
amortiguamiento en nodos y muelles. Por otro lado, podremos añadir a este componente
diversos 3DObjects que actuaran de fijadores para los nodos. Para ello se añadirán una
referencia a ellos en la lista disponible en los parámetros del componente (fixers). Para
comenzar y parar la simulación se utilizará la tecla P, tal cual lo hicimos en clase.

## Relación componentes implementados

El componente principal es MassSpringCloth, el cual es una modificación del implementado en
clase durante los ejercicios con esferas y cilindros. Las modificaciones se han basado en
acceder a la malla del objeto al que hemos añadido el componente, para poder crear los nodos
en las posiciones de sus vértices y los muelles de tracción y flexión que los unen. Una vez
creados, se calculan las fuerzas que se aplican y se actualiza la posición de los vértices de la
malla para aplicar el efecto de tela a nuestro objeto 3D.

Los componentes Node y Spring han sido modificados, quitando el MonoBehaviour para
utilizarlos como clases, ya que no forman parte de la escena como objetos. Se han creado
constructores en ambas clases para poder pasarles los parámetros definidos en el componente
de MassSpringCloth y el método de Compute Forces ha sido modificado en ambos
componentes para simular el fenómeno de amortiguamiento.

Para la realización del requisito 3 de la práctica se ha definido una clase Edge, tal cual se
explica en el enunciado de la práctica. Esto nos permite detectar aristas duplicadas a la hora de
crear los muelles de flexión. Simplemente se ha definido un constructor que permita ordenar
los Vertex A y Vertex B de cada uno de los objetos de la clase para posteriormente ordenar
todos los elementos y detectar estas aristas duplicadas. Para poder realizar esta ordenación de
todos los elementos se ha creado una clase EdgeComparer, la cual hereda de IComparer e
implementa el método Compare, para poder realizar el Sort de la lista de elementos Edge que
creamos en el componente MassSpringCloth.


