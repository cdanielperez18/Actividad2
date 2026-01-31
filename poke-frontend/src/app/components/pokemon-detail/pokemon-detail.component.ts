import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { PokemonService } from '../../services/pokemon.service';
import { PokemonDetail } from '../../models/pokemon.model';

@Component({
  selector: 'app-pokemon-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './pokemon-detail.component.html',
  styleUrl: './pokemon-detail.component.css'
})
export class PokemonDetailComponent implements OnInit {
  pokemon: PokemonDetail | null = null;
  isLoading: boolean = true;
  hasError: boolean = false;
  errorMessage: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private pokemonService: PokemonService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    
    if (id) {
      this.loadPokemonDetail(+id);
    } else {
      this.hasError = true;
      this.errorMessage = 'ID de Pokémon no válido';
      this.isLoading = false;
    }
  }

  /**
   * Cargar detalle del Pokémon
   */
  loadPokemonDetail(id: number): void {
    this.isLoading = true;
    this.hasError = false;

    this.pokemonService.getPokemonById(id).subscribe({
      next: (pokemon: PokemonDetail) => {
        this.pokemon = pokemon;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Error al cargar Pokémon:', error);
        this.hasError = true;
        this.errorMessage = 'No se pudo cargar la información del Pokémon';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  /**
   * Volver al listado
   */
  goBack(): void {
    this.router.navigate(['/pokemon']);
  }

  /**
   * Obtener clase CSS según el tipo
   */
  getTypeClass(type: string): string {
    const typeClasses: { [key: string]: string } = {
      'normal': 'badge bg-secondary',
      'fire': 'badge bg-danger',
      'water': 'badge bg-primary',
      'electric': 'badge bg-warning text-dark',
      'grass': 'badge bg-success',
      'ice': 'badge bg-info',
      'fighting': 'badge bg-danger',
      'poison': 'badge bg-purple',
      'ground': 'badge bg-brown',
      'flying': 'badge bg-info',
      'psychic': 'badge bg-pink',
      'bug': 'badge bg-success',
      'rock': 'badge bg-secondary',
      'ghost': 'badge bg-dark',
      'dragon': 'badge bg-primary',
      'dark': 'badge bg-dark',
      'steel': 'badge bg-secondary',
      'fairy': 'badge bg-pink'
    };

    return typeClasses[type.toLowerCase()] || 'badge bg-secondary';
  }

  /**
   * Obtener color de barra según el stat
   */
  getStatColor(statName: string): string {
    const colors: { [key: string]: string } = {
      'hp': 'bg-success',
      'attack': 'bg-danger',
      'defense': 'bg-primary',
      'special-attack': 'bg-warning',
      'special-defense': 'bg-info',
      'speed': 'bg-secondary'
    };

    return colors[statName.toLowerCase()] || 'bg-secondary';
  }

  /**
   * Obtener porcentaje para barra de progreso
   */
  getStatPercentage(baseStat: number): number {
    return Math.min((baseStat / 255) * 100, 100);
  }

  /**
   * Capitalizar primera letra
   */
  capitalize(text: string): string {
    return text.charAt(0).toUpperCase() + text.slice(1);
  }

  /**
   * Formatear nombre de stat
   */
  formatStatName(name: string): string {
    return name.split('-').map(word => this.capitalize(word)).join(' ');
  }
}
