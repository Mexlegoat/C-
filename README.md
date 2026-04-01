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

## Détails Techniques :
* **Héritage & JSON** : Utilisation de `[JsonDerivedType]` pour sauvegarder correctement les classes enfants (Jeu, Travail, Multimedia).
* **Logique de Navigation** : Utilisation de `Directory.GetParent` pour remonter proprement dans l'arborescence des dossiers.
* **Ouverture Windows Explorer** : Intégration de `Process.Start` pour ouvrir le dossier réel au lieu d'un simple sélecteur.
* **Code Explicite** : Remplacement des opérateurs complexes (`??`, `?`) par des structures `if` pour une meilleure lisibilité.

## Étapes à faire :
* [ ] **Tests globaux** : Vérifier la robustesse de chaque bouton et les cas d'erreurs.
* [ ] **Nettoyage Final** : Optimisation des dernières méthodes de tri.
* [ ] **D'autres idées dans le futur...**
