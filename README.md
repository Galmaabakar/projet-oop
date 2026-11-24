# projet-oop

Application Windows Forms (C#, .NET Framework 4.7.2) réalisée pour un cours de
programmation orientée objet. Un même écran de connexion donne accès à quatre
espaces selon le type d'utilisateur : étudiant, bibliothèque, magasin et
dépôt d'outils. Un écran administrateur affiche l'historique de toutes les
opérations.

## Lancer le projet

Ouvrir `the_project/the_project.csproj` dans Visual Studio, puis appuyer sur F5.

## Comptes de démonstration

Le numéro, le mot de passe et le type doivent correspondre au même compte.
Le nom et le prénom sont libres, mais obligatoires.

| Type                 | Numéro | Mot de passe |
|----------------------|--------|--------------|
| `ogrenci`            | 1001   | 12345678     |
| `ogrenci`            | 1002   | 87654321     |
| `personel_kutuphane` | 2001   | 1234567      |
| `personel_reyon`     | 3001   | 123456       |
| `personel_alet`      | 4001   | 12345        |
| `yonetici`           | 9001   | admin123     |

Les comptes sont définis dans `the_project/KullaniciDeposu.cs`.
Pour l'écran administrateur, choisir le type `yonetici` (ou cliquer sur
**Yoneticiyim**, qui le sélectionne), puis se connecter comme les autres.

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
  `Satis`, `Alet`…). Ce sont des `BindingList`, donc l'écran administrateur se
  met à jour tout seul.
