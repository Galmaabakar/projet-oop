# projet-oop

Application Windows Forms (C#, .NET Framework 4.7.2) réalisée pour un cours de
programmation orientée objet. Un même écran de connexion donne accès à quatre
espaces selon le type d'utilisateur : étudiant, bibliothèque, magasin et
dépôt d'outils. Un écran administrateur affiche l'historique de toutes les
opérations.

## Lancer le projet

Ouvrir `the_project/the_project.csproj` dans Visual Studio, puis appuyer sur F5.
À la première compilation, Visual Studio télécharge automatiquement le paquet
NuGet `Microsoft.Data.Sqlite` (une connexion internet est nécessaire la première fois).

## Comptes de démonstration

Au premier lancement, l'application crée ces comptes. Le numéro, le mot de passe
et le type doivent correspondre au même compte. Le nom et le prénom sont libres,
mais obligatoires.

| Type                 | Numéro | Mot de passe |
|----------------------|--------|--------------|
| `ogrenci`            | 1001   | 12345678     |
| `ogrenci`            | 1002   | 87654321     |
| `personel_kutuphane` | 2001   | 1234567      |
| `personel_reyon`     | 3001   | 123456       |
| `personel_alet`      | 4001   | 12345        |
| `yonetici`           | 9001   | admin123     |

Pour l'écran administrateur, choisir le type `yonetici` (ou cliquer sur
**Yoneticiyim**, qui le sélectionne), puis se connecter comme les autres.

Ces mots de passe ne figurent **que dans ce README** : le code et la base de
données ne contiennent que leurs empreintes (voir plus bas).

### Créer d'autres comptes

L'administrateur peut, dans le panneau **Hesap yonetimi** de son écran :
- créer un compte (numéro, type, mot de passe d'au moins 5 caractères) ;
- changer le mot de passe d'un compte, par exemple si quelqu'un l'a oublié.

La liste des comptes affiche les numéros et les types, jamais les mots de passe.
C'est l'administrateur qui communique son mot de passe à chaque nouvel utilisateur.

## Base de données

Les données sont enregistrées dans une base **SQLite** : un seul fichier,
`veritabani.db`, créé automatiquement à côté du `.exe` (`bin\Debug\`). Il n'y a
aucun serveur à installer.

- Pour repartir de zéro (par exemple si le mot de passe administrateur a été
  oublié), il suffit de fermer l'application et de supprimer `veritabani.db`.
  Les comptes de démonstration seront recréés au lancement suivant.
- Pour regarder le contenu de la base, on peut utiliser
  [DB Browser for SQLite](https://sqlitebrowser.org/).
- Les tables sont créées dans `Veritabani.cs`. La lecture et l'écriture des
  données sont dans `Veriler.cs`, et les comptes dans `KullaniciDeposu.cs`.
- Les requêtes utilisent toujours des paramètres (`$no`), jamais de
  concaténation, ce qui protège contre l'injection SQL.

## Mots de passe

Les mots de passe ne sont jamais enregistrés. Pour chaque compte, la base garde :
- un **sel** : des octets aléatoires, différents pour chaque compte ;
- une **empreinte** calculée avec PBKDF2-SHA256 (100 000 itérations) à partir
  du mot de passe et du sel.

À la connexion, on recalcule l'empreinte de ce qui a été tapé et on la compare.
Une empreinte ne permet pas de retrouver le mot de passe : même avec le fichier
de la base, un pirate ne voit pas les mots de passe. Le code est dans
`SifreGuvenligi.cs`.

## Fonctionnalités

- **Étudiant (`ogrenci`)** : choisir des cours dans le catalogue ou en ajouter.
  La limite est de 39 crédits par étudiant, vérifiée avant chaque ajout.
- **Bibliothèque (`personel_kutuphane`)** : ajouter un livre au catalogue et
  enregistrer les livres remis.
- **Magasin (`personel_reyon`)** : ajouter des produits en stock au panier, puis
  calculer le total du ticket. Le panier repart à zéro pour le client suivant.
- **Dépôt d'outils (`personel_alet`)** : prêter un outil à quelqu'un et
  enregistrer son retour.
- **Administrateur (`yonetici`)** : connexions, cours choisis, livres remis,
  produits vendus, tickets et prêts d'outils.

## Organisation du code

```
giris (interface : ad, soyad)
  └─ gecis (classe abstraite : numéro, connexion, kim, EkranOlustur)
       ├─ ogrenci                  → ogrenciform
       ├─ personel_kutuphaneclass  → personel_kutuphane
       ├─ reyonclass               → personel_reyon
       ├─ personel_aletclass       → personel_alet
       └─ yoneticiclass            → yonetici
```

- **Héritage** : tout ce qui est commun (numéro, nom, connexion) est dans
  `gecis`. Chaque sous-classe ne définit que son type et son propre comportement,
  par exemple `ogrenci.DersAl` ou `reyonclass.SatisYap`.
- **Polymorphisme** : `gecis.EkranOlustur()` ouvre l'écran adapté au type
  d'utilisateur, et `gecis.Olustur(...)` (une fabrique) crée le bon objet.
  `Form1` n'a donc plus besoin d'un `if/else` par type.
- **Données** : `Veriler` contient des listes typées (`Ders`, `Kitap`, `Urun`,
  `Satis`, `Alet`…), chargées depuis la base au démarrage. Chaque ajout est
  d'abord écrit dans la base, puis dans la liste. Ce sont des `BindingList`,
  donc l'écran administrateur se met à jour tout seul.
