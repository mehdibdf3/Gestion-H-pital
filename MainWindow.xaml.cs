using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static Gestion_Hôpital.MainWindow;

namespace Gestion_Hôpital
{

    public partial class MainWindow : Window
    {
        // Déclarations des variables
        int indexeSpe;
        int indexeMed;
        public ObservableCollection<Specialite> ListeSpecialite { get; set; } = new ObservableCollection<Specialite>();

        // Classe représentant une spécialité
        public class Specialite
        {
            public int ID { get; set; }
            public string Nom { get; set; }
            public string Description { get; set; }

        }

        public ObservableCollection<Medecin> ListeMedecin { get; set; } = new ObservableCollection<Medecin>();

        // Classe représentant un médecin
        public class Medecin
        {
            public int ID { get; set; }
            public string Nom { get; set; }
            public string Prenom { get; set; }

            public string Specialite { get; set; }

            public string Telephone { get; set; }

            public string Salaire { get; set; }

        }
        public MainWindow()
        {
            InitializeComponent();


        }
        // Gestion de l'ajout d'une spécialité
        private void btnAjouterSpecialite_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(txtNomSpecialite.Text) || string.IsNullOrWhiteSpace(txtDescriptionSpecialite.Text))
                {
                    MessageBox.Show("Veuillez remplir tous les champs avant d'ajouter une nouvelle spécialité.", "Champs requis manquants", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Specialite laSpecialite = new Specialite
                {
                    ID = ListeSpecialite.Any() ? ListeSpecialite.Max(s => s.ID) + 1 : 1,
                    Nom = txtNomSpecialite.Text,
                    Description = txtDescriptionSpecialite.Text
                };

                ListeSpecialite.Add(laSpecialite);

                dgSpecialite.Items.Add(new
                {
                    IDspecialite = laSpecialite.ID,
                    Nomspecialite = laSpecialite.Nom,
                    Detailspecialite = laSpecialite.Description,
                });
                cbSpecialiteMed.Items.Add(laSpecialite.Nom);
                cbSpecialiteConsultation.Items.Add(laSpecialite.Nom);

                statusMessage.Text = "Insertion Specialite";
                statusMessage.Foreground = Brushes.Green;
                statusAction.Text = "OK";
            }
            catch (Exception exp)
            {
                statusMessage.Text = exp.ToString();
                statusAction.Text = "erreur";
                statusAction.Foreground = Brushes.Red;
            }
        }

        // Gestion de la modification d'une spécialité
        private void btnModifierSpecialite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Specialite specia_aModifier = ListeSpecialite[indexeSpe];


                specia_aModifier.Nom = txtNomSpecialite.Text;
                specia_aModifier.Description = txtDescriptionSpecialite.Text;

                dgSpecialite.Items.RemoveAt(indexeSpe);

                dgSpecialite.Items.Insert(indexeSpe, new
                {
                    IDspecialite = specia_aModifier.ID,
                    Nomspecialite = specia_aModifier.Nom,
                    Detailspecialite = specia_aModifier.Description
                });

                dgSpecialite.Items.Refresh();

                cbSpecialiteMed.Items.RemoveAt(indexeSpe);
                cbSpecialiteMed.Items.Insert(indexeSpe, specia_aModifier.Nom);
                cbSpecialiteMed.Items.Refresh();

                cbSpecialiteConsultation.Items.RemoveAt(indexeSpe);
                cbSpecialiteConsultation.Items.Insert(indexeSpe, specia_aModifier.Nom);
                cbSpecialiteConsultation.Items.Refresh();


                statusMessage.Text = "Modification Specialite";
                statusMessage.Foreground = Brushes.Green;
                statusAction.Text = "OK";
            }
            catch (Exception ex)
            {
                statusMessage.Text = ex.ToString();
                statusAction.Text = "erreur";
                statusAction.Foreground = Brushes.Red;
            }
        }

        // Gestion de la sélection d'une spécialité dans le DataGrid

        private void dgSpecialite_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic ligne_selectionnee = dgSpecialite.SelectedItem;

            if (ligne_selectionnee != null)
            {

                txtNomSpecialite.Text = ligne_selectionnee.Nomspecialite.ToString();
                txtDescriptionSpecialite.Text = ligne_selectionnee.Detailspecialite.ToString();

                indexeSpe = dgSpecialite.SelectedIndex;
            }
        }

        // Gère la suppression d'une spécialité
        private void btnSupprimerSpecialite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dynamic Selected = dgSpecialite.SelectedItem;

                if (Selected != null)
                {
                    bool specialiteUtilisee = ListeMedecin.Any(medecin => medecin.Specialite == Selected.Nomspecialite.ToString());

                    if (specialiteUtilisee)
                    {
                        MessageBox.Show("Impossible de supprimer la spécialité car elle est utilisée par au moins un médecin.", "Erreur de suppression", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {

                        var specialiteASupprimer = ListeSpecialite.FirstOrDefault(specialite => specialite.Nom == Selected.Nomspecialite.ToString());
                        if (specialiteASupprimer != null)
                        {
                            ListeSpecialite.Remove(specialiteASupprimer);
                        }

                        dgSpecialite.Items.RemoveAt(indexeSpe);
                        cbSpecialiteMed.Items.Remove(Selected.Nomspecialite.ToString());
                        cbSpecialiteConsultation.Items.Remove(Selected.Nomspecialite.ToString());
                        dgSpecialite.Items.Refresh();
                        cbSpecialiteMed.Items.Refresh();
                        cbSpecialiteConsultation.Items.Refresh();

                        statusMessage.Text = "Suppression Specialite";
                        statusMessage.Foreground = Brushes.Green;
                        statusAction.Text = "OK";
                    }
                }
            }
            catch (Exception ex)
            {
                statusMessage.Text = ex.ToString();
                statusAction.Text = "erreur";
                statusAction.Foreground = Brushes.Red;
            }
        }

        // Gère l'ajout d'un médecin
        private void btnAjouterMed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNomMed.Text) || string.IsNullOrWhiteSpace(txtPrenomMed.Text) ||
                    cbSpecialiteMed.SelectedItem == null || string.IsNullOrWhiteSpace(txtTelMed.Text) || string.IsNullOrWhiteSpace(txtSalaireMed.Text))
                {
                    MessageBox.Show("Veuillez remplir tous les champs avant d'ajouter un nouveau médecin.", "Champs requis manquants", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Medecin lemedecin = new Medecin
                {
                    ID = ListeMedecin.Any() ? ListeMedecin.Max(m => m.ID) + 1 : 1,
                    Nom = txtNomMed.Text,
                    Prenom = txtPrenomMed.Text,
                    Specialite = cbSpecialiteMed.SelectedItem.ToString(),
                    Telephone = txtTelMed.Text,
                    Salaire = txtSalaireMed.Text
                };

                ListeMedecin.Add(lemedecin);

                dgMedecin.Items.Add(new
                {
                    IDMed = lemedecin.ID,
                    nomMed = lemedecin.Nom,
                    prenomMed = lemedecin.Prenom,
                    speciaMed = lemedecin.Specialite,
                    telMed = lemedecin.Telephone,
                    salaireMed = lemedecin.Salaire
                });

                statusMessage.Text = "Insertion Medecin";
                statusMessage.Foreground = Brushes.Green;
                statusAction.Text = "OK";
            }
            catch (Exception ex)
            {
                statusMessage.Text = ex.ToString();
                statusAction.Text = "erreur";
                statusAction.Foreground = Brushes.Red;
            }
        }

        // Gère la modification d'un médecin
        private void btnModifierMed_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Medecin med_aModifier = ListeMedecin[indexeMed];

                med_aModifier.Nom = txtNomMed.Text;
                med_aModifier.Prenom = txtPrenomMed.Text;
                med_aModifier.Specialite = cbSpecialiteMed.SelectedItem.ToString();
                med_aModifier.Telephone = txtTelMed.Text;
                med_aModifier.Salaire = txtSalaireMed.Text;




                dgMedecin.Items.RemoveAt(indexeMed);

                dgMedecin.Items.Insert(indexeSpe, new
                {
                    IDMed = med_aModifier.ID,
                    nomMed = med_aModifier.Nom,
                    prenomMed = med_aModifier.Prenom,
                    speciaMed = med_aModifier.Specialite,
                    telMed = med_aModifier.Telephone,
                    salaireMed = med_aModifier.Salaire
                });

                dgMedecin.Items.Refresh();


                statusMessage.Text = "Modification Medecin";
                statusMessage.Foreground = Brushes.Green;
                statusAction.Text = "OK";
            }
            catch (Exception ex)
            {
                statusMessage.Text = ex.ToString();
                statusAction.Text = "erreur";
                statusAction.Foreground = Brushes.Red;
            }
        }
        // Gère la sélection d'un médecin dans le DataGrid
        private void dgMedecin_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic ligne_selec2 = dgMedecin.SelectedItem;

            if (ligne_selec2 != null)
            {
                txtNomMed.Text = ligne_selec2.nomMed.ToString();
                txtPrenomMed.Text = ligne_selec2.prenomMed.ToString();
                txtSalaireMed.Text = ligne_selec2.salaireMed.ToString();
                txtTelMed.Text = ligne_selec2.telMed.ToString();
                if (cbSpecialiteMed.Items.Contains(ligne_selec2.speciaMed))
                {
                    cbSpecialiteMed.SelectedItem = ligne_selec2.speciaMed;
                }

                indexeMed = dgMedecin.SelectedIndex;
            }
        }
        // Gère la suppression d'un médecin
        private void btnSupprimerMed_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dynamic Selected2 = dgMedecin.SelectedItem;

                if (Selected2 != null)
                {
                    var medecinASupprimer = ListeMedecin.FirstOrDefault(medecin => medecin.Nom == Selected2.nomMed.ToString() && medecin.Prenom == Selected2.prenomMed.ToString());
                    if (medecinASupprimer != null)
                    {
                        ListeMedecin.Remove(medecinASupprimer);
                    }

                    dgMedecin.Items.RemoveAt(indexeMed);
                    dgMedecin.Items.Refresh();

                    statusMessage.Text = "Suppression Medecin";
                    statusMessage.Foreground = Brushes.Green;
                    statusAction.Text = "OK";


                }
            }
            catch (Exception ex)
            {
                statusMessage.Text = ex.ToString();
                statusAction.Text = "erreur";
                statusAction.Foreground = Brushes.Red;
            }
        }
        // Gère le changement de sélection dans la ComboBox de spécialité pour les consultations
        private void cbSpecialiteConsultation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSpecialiteConsultation.SelectedItem != null)
            {
                string specialiteSelectionnee = cbSpecialiteConsultation.SelectedItem.ToString();
                var medecinsPourSpecialite = ListeMedecin.Where(medecin => medecin.Specialite == specialiteSelectionnee).ToList();

                dgConsultation.ItemsSource = null;
                dgConsultation.ItemsSource = medecinsPourSpecialite;
            }

        }

    }
}
