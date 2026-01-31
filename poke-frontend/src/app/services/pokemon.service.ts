import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PokemonListResponse, PokemonDetail } from '../models/pokemon.model';

@Injectable({
    providedIn: 'root',
})
export class PokemonService {
    private apiUrl = 'http://localhost:5262/api/pokemon'; // Ajusta el puerto si es necesario

    constructor(private http: HttpClient) { }

    /**
   * Obtiene lista paginada de Pokémon
   */
    getPokemon(limit: number, offset: number, name?: string, type?: string): Observable<PokemonListResponse> {
        let params = new HttpParams()
            .set('limit', limit.toString())
            .set('offset', offset.toString());

        if (name && name.trim() !== '') {
            params = params.set('name', name.trim());
        }

        if (type && type.trim() !== '') {
            params = params.set('type', type.trim());
        }

        return this.http.get<PokemonListResponse>(this.apiUrl, { params });
    }

    /**
     * Obtiene detalle de un Pokémon por ID
     */
    getPokemonById(id: number): Observable<PokemonDetail> {
        return this.http.get<PokemonDetail>(`${this.apiUrl}/${id}`);
    }

    /**
     * Obtiene detalle de un Pokémon por nombre
     */
    getPokemonByName(name: string): Observable<PokemonDetail> {
        return this.http.get<PokemonDetail>(`${this.apiUrl}/${name}`);
    }
}
