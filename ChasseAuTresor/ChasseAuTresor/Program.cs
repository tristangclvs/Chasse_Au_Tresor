using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChasseAuTresor
{
    class Program
    {
        static void Main(string[] args)
        {


            /*demander position du premier coup
             * appeler fonctions placement d'items
             * appeler fonction coup */
            Console.Write("Choisissez le numéro de ligne : ");
            int ligne = int.Parse(Console.ReadLine());

            Console.Write("Choisissez le numéro de colonne : ");
            int colonne = int.Parse(Console.ReadLine());

            Console.ReadLine();
        }

        //Sous programmes --------------------------------------------------------------------------------------

        static string[,] Initialiser()
        /* Demande taille du plateau au joueur 
         * et crée le tableau*/
        {
            Console.WriteLine("Veuillez indiquez les dimensions de la grille\nCombien de lignes voulez vous?: ");
            int lignesGrille = int.Parse(Console.ReadLine());

            Console.WriteLine("Combien de colonnes voulez vous?: ");
            int colonnesGrille = int.Parse(Console.ReadLine());

            string[,] grille = new string[lignesGrille, colonnesGrille];
            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    grille[i, j] = " ND ";
                }
            }
            Console.WriteLine();
            AfficherGrille(grille);
            return grille;
        }

        static void AfficherGrille(string[,] tab)
        //Affiche la grille de jeu
        {
            for (int i = 0; i < tab.GetLength(0); i++)
            {
                for (int j = 0; j < tab.GetLength(1); j++)
                {
                    Console.Write(tab[i, j]);
                }
                Console.WriteLine();
            }

        }

        // =================================================
        static int[,] PlacerMines(string[,] grille)
        // Place les mines 
        {
            Random rng = new Random();
            int nbMines = rng.Next(grille.GetLength(0) / 2, grille.GetLength(0) * grille.GetLength(1) / 2 + 1); // + 1 car on veut la moitié pile du tableau et pas la moitié - 1
            int[,] positionsMines = new int[nbMines, 2];
            /* matrice des positions des mines 
             * |x,y| 1ere mine
             * | , | 2ème mine etc*/

            int indice = 0;
            for (int i = 0; i < positionsMines.GetLength(0); i++)
            {
                //créer les coordonnées d'UNE MINE
                indice = i;
                int x = rng.Next(0, grille.GetLength(0) + 1);
                int y = rng.Next(0, grille.GetLength(1) + 1);
                for (int j = indice; j >= 0; j--)
                {
                    while (positionsMines[j, 0] == x && positionsMines[j, 1] == y)
                    {
                        x = rng.Next(0, grille.GetLength(0) + 1);
                        y = rng.Next(0, grille.GetLength(1) + 1);
                    }
                }
                // Fin de la création de coordonnées

                // Début positionnement
                positionsMines[i, 0] = rng.Next(0, grille.GetLength(0) + 1); // coordonnées x des mines
                positionsMines[i, 1] = rng.Next(0, grille.GetLength(1) + 1); // coordonnées y des mines
            }
            return positionsMines;

        }
        static int[,] PlacerTresors(string[,] grille)
        // Place les trésors 
        {
            Random rng = new Random();
            int nbTresors = rng.Next(1, 4); // Entre 1 et 3 trésors
            int[,] positionsTresors = new int[nbTresors, 2];
            /* matrice des positions des trésors 
             * |x,y| 1er trésor
             * | , | 2ème trésor etc*/

            int indice = 0;
            for (int i = 0; i < positionsTresors.GetLength(0); i++)
            {
                //créer les coordonnées d'UN TRESOR
                indice = i;
                int x = rng.Next(0, grille.GetLength(0) + 1);
                int y = rng.Next(0, grille.GetLength(1) + 1);
                for (int j = indice; j >= 0; j--)
                {
                    while (positionsTresors[j, 0] == x && positionsTresors[j, 1] == y)
                    {
                        x = rng.Next(0, grille.GetLength(0) + 1);
                        y = rng.Next(0, grille.GetLength(1) + 1);
                    }
                }
                // Fin de la création de coordonnées

                // Début positionnement
                positionsTresors[i, 0] = rng.Next(0, grille.GetLength(0) + 1); // coordonnées x des trésors
                positionsTresors[i, 1] = rng.Next(0, grille.GetLength(1) + 1); // coordonnées y des trésors
            }
            return positionsTresors;
        }
        // =================================================
        static void Jouer(string[,] grille)
        // Demande au joueur de jouer un coup et le joue
        { // Penser à faire ligne -1 et colonne -1 pour la position du coup.

            int[,] positionsMines = PlacerMines(grille);
            int[,] positionsTresors = PlacerTresors(grille);




            //EstPerdue(ligne,  colonne, positionsMines, grille);

        }
        static void EstPerdue(int ligne, int colonne, int[,] positionsMines, string[,] grille)
        // Détecte si le joueur a touché une mine et arrête la partie ou continue la partie sinon
        {
            for (int i = 0; i < positionsMines.GetLength(0); i++)
            {
                if (positionsMines[i, 0] == ligne && positionsMines[i, 1] == colonne)
                {
                    Console.WriteLine("💣 BOUMMM 💣 \nTu es mort ☠ !!!"); // Si on a le temps récapitulaif de partie : nb de coups et nb de trésors gagnés
                }
                else { Jouer(grille); }
            }
        }

        static bool EstGagnee(string[,] grille, int[,] positionsMines)
        // Détecte si le joueur a gagné et arrête la partie
        {
            bool win = false;
            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    if (grille[i, j] == " ND " && CheckerPositions(i, j, positionsMines) == false)
                    {
                        return win;
                    }
                }
            }
            win = true;
            return win;
        }
        static bool CheckerPositions(int i, int j, int[,] positionsItems)
        // renvoie true si la position vérifiée est une mine ou un trésor, false sinon
        {
            bool reponse = false;
            for (int indice = 0; indice < positionsItems.GetLength(0); indice++)
            {
                if (positionsItems[indice, 0] == i && positionsItems[indice, 1] == j)
                {
                    reponse = true;
                    return reponse;
                }
            }
            return reponse;
        }

    }
}
