import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { PokemonService } from '../../services/pokemon.service';
import { PokemonListResponse, PokemonListItem } from '../../models/pokemon.model';

@Component({
    selector: 'app-pokemon-list',
    standalone: true,
    imports: [CommonModule, RouterModule, FormsModule],
    templateUrl: './pokemon-list.component.html',
    styleUrl: './pokemon-list.component.css',
})
export class PokemonListComponent implements OnInit {
    pokemon: PokemonListItem[] = [];
    totalCount: number = 0;
    limit: number = 20;
    offset: number = 0;
    totalPages: number = 0;
    currentPage: number = 1;

    // Filtros
    nameInput: string = ''; 
    nameFilter: string = '';
    typeFilter: string = '';

    // Lista de tipos de Pokémon  ← AGREGAR TODO ESTO
    pokemonTypes = [
        { name: 'normal', color: '#A8A878', emoji: '⚪' },
        { name: 'fire', color: '#F08030', emoji: '🔥' },
        { name: 'water', color: '#6890F0', emoji: '💧' },
        { name: 'electric', color: '#F8D030', emoji: '⚡' },
        { name: 'grass', color: '#78C850', emoji: '🌿' },
        { name: 'ice', color: '#98D8D8', emoji: '❄️' },
        { name: 'fighting', color: '#C03028', emoji: '🥊' },
        { name: 'poison', color: '#A040A0', emoji: '☠️' },
        { name: 'ground', color: '#E0C068', emoji: '🏜️' },
        { name: 'flying', color: '#A890F0', emoji: '🕊️' },
        { name: 'psychic', color: '#F85888', emoji: '🔮' },
        { name: 'bug', color: '#A8B820', emoji: '🐛' },
        { name: 'rock', color: '#B8A038', emoji: '🪨' },
        { name: 'ghost', color: '#705898', emoji: '👻' },
        { name: 'dragon', color: '#7038F8', emoji: '🐉' },
        { name: 'dark', color: '#705848', emoji: '🌙' },
        { name: 'steel', color: '#B8B8D0', emoji: '⚙️' },
        { name: 'fairy', color: '#EE99AC', emoji: '🧚' }
    ];

    // Estados
    isLoading: boolean = false;
    hasError: boolean = false;
    errorMessage: string = '';
    Math = Math;

    constructor(
        private pokemonService: PokemonService,
        private cdr: ChangeDetectorRef,
    ) { }

    ngOnInit(): void {
        this.loadPokemon();
    }

    /**
     * Cargar Pokémon desde el backend
     */
    loadPokemon(): void {
        this.isLoading = true;
        this.hasError = false;

        this.pokemonService.getPokemon(
            this.limit,
            this.offset,
            this.nameFilter || undefined,
            this.typeFilter || undefined
        ).subscribe({
            next: (response: PokemonListResponse) => {
                this.pokemon = response.pokemon;
                this.totalCount = response.totalCount;
                this.totalPages = Math.ceil(this.totalCount / this.limit);
                this.currentPage = Math.floor(this.offset / this.limit) + 1;
                this.isLoading = false;

                if (this.pokemon.length === 0) {
                    this.errorMessage = 'No se encontraron Pokémon';
                }

                this.cdr.detectChanges();
            },
            error: (error) => {
                console.error('Error al cargar Pokémon:', error);
                this.hasError = true;
                this.errorMessage = 'Error al cargar los Pokémon. Intenta de nuevo.';
                this.isLoading = false;
                this.cdr.detectChanges();
            }
        });
    }

    /**
     * Aplicar filtros
     */
    applyFilters(): void {
        this.nameFilter = this.nameInput;
        this.offset = 0;
        this.currentPage = 1;
        this.loadPokemon();
        this.cdr.detectChanges();
    }

    /**
     * Limpiar filtros
     */
    clearFilters(): void {
        this.nameInput = '';
        this.nameFilter = '';
        this.typeFilter = '';
        this.offset = 0;
        this.currentPage = 1;
        this.loadPokemon();
    }

    /**
     * Cambiar cantidad de Pokémon por página
     */
    changeLimit(): void {
        this.limit = Number(this.limit);
        this.offset = 0;
        this.currentPage = 1;
        this.loadPokemon();
        this.cdr.detectChanges();
    }

    /**
    * Filtrar por tipo de Pokémon
    */
    filterByType(type: string): void {
        if (this.typeFilter === type) {
            this.typeFilter = '';
        } else {
            this.typeFilter = type;
        }

        this.offset = 0;
        this.currentPage = 1;
        this.cdr.detectChanges();
        this.loadPokemon();
        this.cdr.detectChanges();
    }

    /**
     * Ir a la página anterior
     */
    previousPage(): void {
        if (this.offset > 0) {
            this.offset -= this.limit;
            this.loadPokemon();
        }
    }

    /**
     * Ir a la página siguiente
     */
    nextPage(): void {
        if (this.offset + this.limit < this.totalCount) {
            this.offset += this.limit;
            this.loadPokemon();
        }
    }

    /**
     * Ir a una página específica
     */
    goToPage(page: number): void {
        this.offset = (page - 1) * this.limit;
        this.loadPokemon();
    }

    /**
     * Obtener array de números de página para mostrar
     */
    getPageNumbers(): number[] {
        const pages: number[] = [];
        const maxPagesToShow = 5;
        let startPage = Math.max(1, this.currentPage - 2);
        let endPage = Math.min(this.totalPages, startPage + maxPagesToShow - 1);

        if (endPage - startPage < maxPagesToShow - 1) {
            startPage = Math.max(1, endPage - maxPagesToShow + 1);
        }

        for (let i = startPage; i <= endPage; i++) {
            pages.push(i);
        }

        return pages;
    }

    /**
     * Obtener clase CSS según el tipo
     */
    getTypeClass(type: string): string {
        const typeClasses: { [key: string]: string } = {
            normal: 'badge bg-secondary',
            fire: 'badge bg-danger',
            water: 'badge bg-primary',
            electric: 'badge bg-warning text-dark',
            grass: 'badge bg-success',
            ice: 'badge bg-info',
            fighting: 'badge bg-danger',
            poison: 'badge bg-purple',
            ground: 'badge bg-brown',
            flying: 'badge bg-info',
            psychic: 'badge bg-pink',
            bug: 'badge bg-success',
            rock: 'badge bg-secondary',
            ghost: 'badge bg-dark',
            dragon: 'badge bg-primary',
            dark: 'badge bg-dark',
            steel: 'badge bg-secondary',
            fairy: 'badge bg-pink',
        };

        return typeClasses[type.toLowerCase()] || 'badge bg-secondary';
    }

    /**
     * Capitalizar primera letra
     */
    capitalize(text: string): string {
        return text.charAt(0).toUpperCase() + text.slice(1);
    }
}
