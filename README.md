# PROJET : Gestionnaire d'Applications
## Créer une application qui permet de gérer les applications ainsi que les exécuter.

## Étapes Réussies :
* [x] **Interface Graphique** : Création de la fenêtre principale et du design WPF.
* [x] **Création des Applications** : Système d'ajout avec ID et horodatage.
* [x] **Visuels** : Affichage dynamique des icônes et images.
* [x] **Collections** : Ajout et gestion dans les listes filtrées (Jeux, Travail, etc.).
* [x] **Sécurité** : Page de Login fonctionnelle avant l'accès au MainWindow.
* [x] **Paramètres** : Interface complète pour les préférences utilisateur.
* [x] **Registre Windows** : Utilisation de la classe Registry pour mémoriser les infos.
* [x] **Exécution** : Lancement des applications externes (Process.Start).
* [x] **Persistance JSON** : Sauvegarde et chargement gérant l'héritage (Polymorphisme).
* [x] **Exploration de Fichiers** : Système de "Browse" ouvrant directement l'explorateur Windows.
* [x] **Navigation de Dossiers** : Calcul dynamique du chemin projet (BaseDirectory).
* [x] **Ajout de 3 paramètres qui fonctionnent correctement:**
* [x] **- Affichage du genre.**
* [x] **- Affichage du type.**
* [x] **- Execution de l'application avec double-clic.**
* [x] **Ajout du bouton Supprimer pour supprimer une application (et actualiser le datagrid pour le mettre à jour).**
* [x] **Ajout du bouton pour créer un type quelconque.**
* [x] **Ajout de la combobox pour sélectionner (ou pas) un type parmis les types que l'utilisateur a créé (et sauvegardé donc dans le .json) avec un bouton pour Modifier le type (Mettre à jour la listbox et le datagrid).**
* [x] **Ajout d'un clic sur la listbox pour permettre à l'utilisateur d'accéder donc à la suppression et à l'ajout d'un type.**
* [x] **Continuation du précédant: Ajout d'une fonction qui permet de remettre la listbox à sa forme de base quand on clique autre part que sur la listbox elle-même.**
* [x] **Modification de la fonction LaunchApp et d'autres fonctions pour rajouter des MessageBoxs pour clarifier et mieux expliquer les erreurs.**



## Détails Techniques :
* **Héritage & JSON** : Utilisation de `[JsonDerivedType]` pour sauvegarder correctement les classes enfants (Jeu, Travail, Multimedia).
* **Logique de Navigation** : Utilisation de `Directory.GetParent` pour remonter proprement dans l'arborescence des dossiers.
* **Ouverture Windows Explorer** : Intégration de `Process.Start` pour ouvrir le dossier réel au lieu d'un simple sélecteur.
* **Code Explicite** : Remplacement des opérateurs complexes (`??`, `?`) par des structures `if` pour une meilleure lisibilité.

## Étapes à faire :
* [ ] **Tests globaux** : Vérifier la robustesse de chaque bouton et les cas d'erreurs.
* [ ] **Nettoyage Final** : Optimisation des dernières méthodes de tri.
* [ ] **Settings** : Faire fonctionner la radio pour le thème (Sombre et clair).
* [ ] **Recherche** : Ajouter une fonction qui permet de rechercher des applications à partir du type de recherche (voir après).
* [ ] **Settings** : Ajouter un autre paramètre pour pouvoir choisir (radio) entre Nom, Genre et Type pour rechercher dans la barre de recherche.
* [ ] **D'autres idées dans le futur...**
