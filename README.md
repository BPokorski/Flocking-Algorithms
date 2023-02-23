# Polski
## Algorytm ławicy
Projekt utworzony w ramach pracy magisterskiej, analizujący wybrane algorytmy ławicy <br/>

W ramach projektu zaimplementowano dwie wersje algorytmu:
- **Podstawowa** - podstawowa wersja algorytmu
- **Drzewo ósemkowe** - Optymalizacja z wykorzystaniem podziału przestrzeni

Oprócz tego, każda wersja algorytmu umożliwia również zmniejsze częstotliwości wyznaczania algorytmu <br/>
poprzez redukcję liczby klatek co którą wykonywany jest algorytm.

W składach aplikacji wchodzi również interfejs użytkownika składajacy się z elementów:
- **Przycisk i pole tekstowe liczby boidów** umożliwiający utworzenie podanej liczby na scenie
- **Przycisk i pole tekstowe liczby przeszkód** pozwalający utworzyć zadaną liczbę przeszkód, którą boidy powinny unikać
- **Licznik FPS** - Pokazujący obecną liczbę klatek
- **Stoper** - mierzący czas w jakim boidy pokonują daną ścieżkę
- **Przycisku Restartu** - umożliwiający restart aplikacji

# English
## flocking-algorithms
Master thesis project. Analysis of different flocking algorithms <br/>

Project consist of two version of flocking algorithm:
- **Basic** - basic implementation of flocking algorithm
- **Octree** - spaced-division based optimisation of algorithm

As well as optimisation with frame reduce computing.

There is also user interface implemented in applichation which consist of:
- **Boid Number Input/Button** which allows user to enter number of boids to spawn
- **Obstacle Number Input/Button** giving user possibilty to spawn number of obstacles that boid should avoid
- **FPS Counter** - which display current number of FPS
- **Timer** - To measure how long boids will go through their path
- **Restart Button** - allowing to restart application
