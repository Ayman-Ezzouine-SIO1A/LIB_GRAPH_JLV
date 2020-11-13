
/*
 * OUTILS POUR COURS ALGO - VINOLA Jean louis Lycée de la CCI de Nimes
 * 
 * VERSION 1.20.01.16
 *   Grille_Vers_BMP : stocke un tableau 2Dim d'entiers en BMP32bits
 *   BMP_Vers_Grille: charge un BMP32bits ou BMP24bits dans un tableau 2Dim
 * 
 */
using System;
using System.Drawing;
using System.Drawing.Imaging;

/*
 * 
 * PENSEZ A :
 * 
 * ==> Aouter ce fichier dans un PRJ Console .Net Framework 
 * 
 * ==> ajouter la référence Syste.Drawing :
 *       Cliquez bouton droit sur Référence dans la fenêtre du Projet
 *       Choisir "Ajouter une référence"
 *       Dans la liste, cochez "System.Drawing"
 *       
 * ==> Accepter le code "unsafe" :
 *      Cliquez bouton droit sur le nom du Projet dans la fenêtre du Projet
 *      Choisir "Propriétés"
 *      Cliquez sur "Build" situé à gauche
 *      Cochez l'option "Autoriser le code Unsafe"
 *      
 *      
 * 
 * */


namespace JLV_TOOLS
{
	public class IMAGE
	{

		struct T_RGB24
		{
#pragma warning disable 0649
			private readonly char R;
			private readonly char G;
			private readonly char B;
			 public int ToInt() { return  (G <<16) | (B<<8) | (R << 0); }
		}



		public static int Donne_Niveau(int P_Point)
			{
				return ( ((P_Point & 0x00FF0000) >> 16) + 
					       ((P_Point & 0x0000FF00) >> 8) + 
							   ((P_Point & 0x000000FF) ) / 3 );

			}


		

		//-----------------------
		public static int[,] BMP_Vers_Grille(string P_Nom)
		{
			int[,] L_Grille = null;
				try {
					Bitmap L_Bitmap = new Bitmap(P_Nom);
					BitmapData L_Data_BMP = L_Bitmap.LockBits(
															 new Rectangle(0, 0, L_Bitmap.Width, L_Bitmap.Height),
															 ImageLockMode.WriteOnly, L_Bitmap.PixelFormat);
			

				
					int L_Largeur = L_Bitmap.Width;
					int L_Hauteur = L_Bitmap.Height;
					L_Grille = new int[L_Largeur, L_Hauteur];
				int L_Offset = 1;
	
				 
				switch (L_Bitmap.PixelFormat) {
					case PixelFormat.Format24bppRgb: L_Offset = 3; break;
					case PixelFormat.Format32bppArgb: L_Offset = 4;  break;
					case PixelFormat.Format32bppPArgb: L_Offset = 4; break;

						// format bmp plugin visual studio
					case PixelFormat.Format32bppRgb: L_Offset = 4; break;

					default: Console.WriteLine("Format non pris en charge. Uniquement BMP 24 ou 32 bits");
						return null;
				}

				unsafe {
					
						void* PT_Data = (void*)L_Data_BMP.Scan0.ToPointer();
					  T_RGB24* PT_RGB = (T_RGB24*)PT_Data;
				

						for (int Colonne = 0; Colonne < L_Largeur; Colonne++) {
							for (int Ligne = 0; Ligne < L_Hauteur; Ligne++) {

							  if (L_Offset == 4) (L_Grille[Colonne, Ligne]) = *(int*)PT_Data ;
							  else(L_Grille[Colonne, Ligne]) = (*(T_RGB24*)PT_Data).ToInt();
												
							  PT_Data = (void*)((int)PT_Data + L_Offset);
							
							}
						}
					}
					L_Bitmap.UnlockBits(L_Data_BMP);
				}
				catch (Exception) {
					Console.WriteLine($"Erreur : Fichier {P_Nom} introuvable");
				}
			
			return L_Grille;
		}
		//-----------------------
		public static void Grille_Vers_BMP(int[,] P_Grille, string P_Nom)
		{
			unsafe {
				int L_Largeur = P_Grille.GetLength(0);
				int L_Hauteur = P_Grille.GetLength(1);
				Bitmap Le_Bitmap = new Bitmap(L_Largeur, L_Hauteur, PixelFormat.Format32bppRgb);


				BitmapData Data_BMP = Le_Bitmap.LockBits(
														 new Rectangle(0, 0, Le_Bitmap.Width, Le_Bitmap.Height),
														 ImageLockMode.WriteOnly, Le_Bitmap.PixelFormat);
				int* PT_Data = (int*)Data_BMP.Scan0.ToPointer();
				for (int Colonne = 0; Colonne < L_Largeur; Colonne++) {
					for (int Ligne = 0; Ligne < L_Hauteur; Ligne++) {
						*PT_Data = P_Grille[Colonne, Ligne];
						PT_Data++;
					}
				}
				Le_Bitmap.UnlockBits(Data_BMP);
				Le_Bitmap.Save(P_Nom);
			}
		}
	}
}
