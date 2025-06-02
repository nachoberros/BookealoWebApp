import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Router, RouterModule } from '@angular/router';
import { Asset } from './assets.models';
import { AssetType } from '../calendars/candelars.model';

@Component({
    selector: 'app-assets',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './assets.component.html',
    styleUrls: ['./assets.component.css']
})
export class AssetsComponent implements OnInit {
    public AssetType = AssetType;
    assets: Asset[] = [];
    loading: boolean = false;

    constructor(
        private http: HttpClient,
        private router: Router
    ) { }

    ngOnInit(): void {
        this.refreshAssets();
    }

    editAsset(asset: Asset) {
        console.log('Edit', asset);
        this.router.navigate(['/assets/edit', asset.id]);
    }

    deleteAsset(asset: Asset) {
        if (!asset) return;

        if (confirm(`Are you sure you want to delete "${asset.name}"?`)) {
            this.assets = this.assets.filter(c => c.id !== asset.id);

            const params = { id: asset.id };

            this.http.delete('/api/asset', { params }).subscribe({
                next: () => this.refreshAssets(),
                error: err => console.error('Asset deletion failed', err)
            });
        }
    }

    refreshAssets() {
        this.loading = true;
        this.http.get<Asset[]>(`/api/asset`).subscribe({
            next: data => {
                this.assets = data;
                setTimeout(() => this.loading = false, 500);
            },
            error: error => {
                console.error('Failed to fetch assets data', error);
                this.loading = false;
            }
        });
    }
}
